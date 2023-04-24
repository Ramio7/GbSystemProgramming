using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private readonly int _port = 5805;
    private const int MaxConnections = 10;
    private int _hostID;
    private int _reliableChannel;
    private bool _isStarted = false;
    private byte _error;
    List<int> _connectionIDs = new();
    Dictionary<int, string> _users = new();

    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new();
        _reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new(cc, MaxConnections);
        _hostID = NetworkTransport.AddHost(topology, _port);
        _isStarted = true;
    }

    public void ShutDownServer()
    {
        if (!_isStarted) return;
        NetworkTransport.RemoveHost(_hostID);
        NetworkTransport.Shutdown();
        _isStarted = false;
    }

    void Update()
    {
        if (!_isStarted) return;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        NetworkEventType recData = NetworkTransport.Receive(
            out int recHostId, 
            out int connectionId, 
            out int channelId, 
            recBuffer, 
            bufferSize, 
            out int dataSize, 
            out _error);
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    _connectionIDs.Add(connectionId);
                    _users.Add(connectionId, string.Empty);
                    SendMessageToAll($"{_users[connectionId]} has connected.");
                    Debug.Log($"{_users[connectionId]} has connected.");
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);

                    if (_users[connectionId] == string.Empty) _users[connectionId] = message;
                    else SendMessageToAll($"{_users[connectionId]}: {message}");

                    Debug.Log($"{_users[connectionId]}: {message}");
                    break;
                case NetworkEventType.DisconnectEvent:
                    SendMessageToAll($"{_users[connectionId]} has disconnected.");
                    Debug.Log($"{_users[connectionId]} has disconnected.");
                    _connectionIDs.Remove(connectionId);
                    _users.Remove(connectionId);
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
            bufferSize, out dataSize, out _error);
        }
    }

    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < _connectionIDs.Count; i++)
        {
            SendMessage(message, _connectionIDs[i]);
        }
    }

    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes($"{message}");
        NetworkTransport.Send(_hostID, connectionID, _reliableChannel, buffer, message.Length *
        sizeof(char), out _error);
        if ((NetworkError)_error != NetworkError.Ok) Debug.Log((NetworkError)_error);
    }
}
