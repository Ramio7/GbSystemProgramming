using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIPresenter : MonoBehaviour
{
    [field: SerializeField] public Button StartServerButton { get; private set; }
    [field: SerializeField] public Button StartClientButton { get; private set; }
    [field: SerializeField] public Button StartHostButton { get; private set; }
    [field: SerializeField] public Button ShutdownButton { get; private set; }
    [field: SerializeField] public TMP_InputField PlayerNameField { get; private set; }

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
        HW5EntryPoint.NetworkManager.StartServer();
        SetButtonsActivity(true);
    }

    private void StartClient()
    {
        HW5EntryPoint.NetworkManager.StartClient();
        SetButtonsActivity(true);
        HW5EntryPoint.NetworkManager.playerName = PlayerNameField.text;
    }

    private void StartHost()
    {
        HW5EntryPoint.NetworkManager.StartHost();
        SetButtonsActivity(true);
        HW5EntryPoint.NetworkManager.playerName = PlayerNameField.text;
    }

    private void ShutDown()
    {
        HW5EntryPoint.NetworkManager.Shutdown();
        SetButtonsActivity(false);
        HW5EntryPoint.NetworkManager.playerName = null;
    }

    private void SetButtonsActivity(bool instanceActivityFlag)
    {
        StartServerButton.gameObject.SetActive(!instanceActivityFlag);
        StartClientButton.gameObject.SetActive(!instanceActivityFlag);
        StartHostButton.gameObject.SetActive(!instanceActivityFlag);
        PlayerNameField.gameObject.SetActive(!instanceActivityFlag);
        ShutdownButton.gameObject.SetActive(instanceActivityFlag);
    }
}
