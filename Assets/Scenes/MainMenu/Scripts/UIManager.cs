using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class UIManager
        : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private UNetTransport transport;
        public int defaultPort = 7777;
        public string localHostAddress = "127.0.0.1";
        public string lobbyScene;

        [SerializeField]
        private UnityEvent OnServerStarted = new UnityEvent();
        [SerializeField]
        private UnityEvent OnConnected = new UnityEvent();

        public void Start()
        {
            networkManager.OnServerStarted += () => {
                OnServerStarted.Invoke();
                if (networkManager.IsHost)
                    OnConnected.Invoke();
            };
            networkManager.OnClientConnectedCallback += (ulong v) => {
                Debug.Log($"Client with id={v} connected!");
                if(!networkManager.IsHost)
                    OnConnected.Invoke(); 
            };
        }

        public void StartClientTMP(TextMeshProUGUI address)
        {
            StartClient(address.text);
        }

        public void StartClient(string address)
        {
            transport.ConnectAddress = address;
            if (!networkManager.StartClient())
                throw new System.InvalidProgramException("Failed to start client!");
        }

        public void StartHosting(TextMeshProUGUI port)
        {
            if (!int.TryParse(port.text, out var portV))
                portV = defaultPort;

            StartHost(portV);
        }

        public void StartHost(int port)
        {
            transport.ServerListenPort = port;
            transport.ConnectAddress = localHostAddress;
            if (!networkManager.StartHost())
                throw new System.InvalidProgramException("Failed to start client!");
        }

        public void EnterLobbyScene()
        {
            networkManager.SceneManager.LoadScene(lobbyScene, LoadSceneMode.Single);
        }
    }
}
