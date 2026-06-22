using Unity.Netcode;
using UnityEngine;

public class ControladorJugadorRed : NetworkBehaviour
{
    [Header("Salud")]
    public NetworkVariable<int> energia = new NetworkVariable<int>(100);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3);

    public override void OnNetworkSpawn()
    {
        // Solo inicializar en el servidor
        if (IsServer)
        {
            energia.Value = 100;
            vidas.Value = 3;
        }
    }

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

    // ===== UI AUTOMÁTICA - SE DIBUJA SOLA EN PANTALLA =====
    void OnGUI()
    {
        // Solo mostrar la UI del DUEÑO de este jugador
        if (!IsOwner) return;

        GUI.Box(new Rect(10, 10, 220, 80), "=== TÚ ===");
        GUI.Label(new Rect(20, 35, 200, 25), $"⚡ ENERGÍA: {energia.Value}");
        GUI.Label(new Rect(20, 55, 200, 25), $"❤️  VIDAS: {vidas.Value}");
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }
}