using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private readonly int _port = 0;
    private readonly int _serverPort = 5805;
    private const int MaxConnections = 10;
    [SerializeField] private TMP_InputField _username;
    private int _hostID;
    private int _reliableChannel;
    private int _connectionID;
    private bool _isConnected = false;
    private byte _error;

    public event Action<string> OnMessageReceive;

    public void Connect()
    {
        if (_username.text == string.Empty) throw new ArgumentNullException("Enter username");
        NetworkTransport.Init();
        ConnectionConfig cc = new();
        _reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new(cc, MaxConnections);
        _hostID = NetworkTransport.AddHost(topology, _port);
        _connectionID = NetworkTransport.Connect(_hostID, "127.0.0.1", _serverPort, 0, out _error);
        if ((NetworkError)_error == NetworkError.Ok)
        {
            _isConnected = true;
        }
        else
            Debug.Log((NetworkError)_error);
    }

    public void Disconnect()
    {
        if (!_isConnected) return;
        NetworkTransport.Disconnect(_hostID, _connectionID, out _error);
        _isConnected = false;
    }

    void Update()
    {
        if (!_isConnected) return;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        NetworkEventType recData = NetworkTransport.Receive(
            out int recHostId, 
            out int connectionId, 
            out int channelId, 
            recBuffer, bufferSize, 
            out int dataSize, 
            out _error);
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    SendMessage(_username.text);
                    Debug.Log($"You have been connected to server.");
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    OnMessageReceive?.Invoke(message);
                    Debug.Log(message);
                    break;
                case NetworkEventType.DisconnectEvent:
                    _isConnected = false;
                    Debug.Log($"You have been disconnected from server.");
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(
                out recHostId, 
                out connectionId, 
                out channelId, 
                recBuffer,
                bufferSize, 
                out dataSize, 
                out _error);
        }
    }

    public void SendMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, message.Length *
        sizeof(char), out _error);
        if ((NetworkError)_error != NetworkError.Ok) Debug.Log((NetworkError)_error);
    }
}
