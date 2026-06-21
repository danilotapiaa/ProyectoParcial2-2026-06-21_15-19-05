using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    private void Start()
    {
        // Conectar el botón de Host
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false); // Oculta el panel negro al empezar
        });

        // Conectar el botón de Cliente
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false); // Oculta el panel negro al empezar
        });
    }
}