using Unity.Netcode;
using UnityEngine;

public class DungeonManagerNet : NetworkBehaviour
{
    public NetworkVariable<int> llavesCompartidas = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Referencias a tus paredes que arrastraremos en el Inspector
    public GameObject puertaCerrada;
    public GameObject puertaAbierta;

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 160, 10, 150, 50), "=== OBJETIVO ===");
        GUI.Label(new Rect(Screen.width - 140, 35, 130, 20), "Llaves: " + llavesCompartidas.Value + " / 5");
    }

    public void SumarLlave()
    {
        if (IsServer)
        {
            llavesCompartidas.Value++;

            if (llavesCompartidas.Value >= 5)
            {
                // El servidor le avisa a todos los jugadores que abran la puerta
                AbrirPuertasClientRpc();
            }
        }
    }

    [ClientRpc]
    private void AbrirPuertasClientRpc()
    {
        if (puertaCerrada != null) puertaCerrada.SetActive(false);
        if (puertaAbierta != null) puertaAbierta.SetActive(true);
    }
}