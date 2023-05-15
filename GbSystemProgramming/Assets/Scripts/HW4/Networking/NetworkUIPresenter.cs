using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIPresenter : MonoBehaviour
{
    [field: SerializeField] public Button StartServerButton { get; private set; }
    [field: SerializeField] public Button StartClientButton { get; private set; }
    [field: SerializeField] public Button StartHostButton { get; private set; }
    [field: SerializeField] public Button ShutdownButton { get; private set; }
    [field: SerializeField] public NetworkManager NetworkManager { get; private set; }

    private void Start()
    {
        SubscribeButtons();
        SetButtonsActivity(false);
    }

    private void OnDestroy()
    {
        UnsubscribeButtons();
    }

    private void SubscribeButtons()
    {
        StartServerButton.onClick.AddListener(StartServer);
        StartClientButton.onClick.AddListener(StartClient);
        StartHostButton.onClick.AddListener(StartHost);
        ShutdownButton.onClick.AddListener(ShutDown);
    }

    private void UnsubscribeButtons()
    {
        StartServerButton.onClick.RemoveAllListeners();
        StartClientButton.onClick.RemoveAllListeners();
        StartHostButton.onClick.RemoveAllListeners();
        ShutdownButton.onClick.RemoveAllListeners();
    }

    private void StartServer()
    {
        NetworkManager.StartServer();
        SetButtonsActivity(true);
    }

    private void StartClient()
    {
        NetworkManager.StartClient();
        SetButtonsActivity(true);
    }

    private void StartHost()
    {
        NetworkManager.StartHost();
        SetButtonsActivity(true);
    }

    private void ShutDown()
    {
        NetworkManager.Shutdown();
        SetButtonsActivity(false);
    }

    private void SetButtonsActivity(bool instanceActivityFlag)
    {
        StartServerButton.gameObject.SetActive(!instanceActivityFlag);
        StartClientButton.gameObject.SetActive(!instanceActivityFlag);
        StartHostButton.gameObject.SetActive(!instanceActivityFlag);
        ShutdownButton.gameObject.SetActive(instanceActivityFlag);
    }
}
