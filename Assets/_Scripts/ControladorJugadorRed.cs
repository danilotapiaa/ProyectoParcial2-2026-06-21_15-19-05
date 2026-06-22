using Unity.Netcode;
using UnityEngine;

public class ControladorJugadorRed : NetworkBehaviour
{
    public NetworkVariable<int> energia = new NetworkVariable<int>(100);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3);

    public void RecibirDanio(int cantidad)
    {
        if (!IsServer) return;
        energia.Value -= cantidad;

        if (energia.Value <= 0)
        {
            vidas.Value--;
            if (vidas.Value > 0)
            {
                energia.Value = 100;
                transform.position = new Vector3(-25f, 0.1f, 50f);
            }
            else
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    void OnGUI()
    {
        if (!IsOwner) return;
        GUI.Box(new Rect(10, 10, 200, 85), "=== ESTADO JUGADOR ===");
        GUI.Label(new Rect(20, 35, 180, 25), $" ENERGIA: {energia.Value}");
        GUI.Label(new Rect(20, 55, 180, 25), $" VIDAS: {vidas.Value}");
    }
}