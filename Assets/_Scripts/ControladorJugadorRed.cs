using Unity.Netcode;
using UnityEngine;

public class ControladorJugadorRed : NetworkBehaviour
{
    [Header("Salud")]
    public NetworkVariable<int> energia = new NetworkVariable<int>(100);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3);

    // Variable para guardar dónde empezamos
    private Vector3 posicionInicial;

    public override void OnNetworkSpawn()
    {
        // Guardamos la posición exacta donde Unity nos hizo aparecer
        posicionInicial = transform.position;

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
                // Le decimos al cliente dueño de este personaje que haga respawn
                EjecutarRespawnClientRpc(posicionInicial);
            }
            else
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    // El servidor le ordena a este cliente específico que se mueva
    [ClientRpc]
    private void EjecutarRespawnClientRpc(Vector3 posDeRespawn)
    {
        if (IsOwner)
        {
            // Apagamos momentáneamente el Rigidbody para que no pelee con el teletransporte
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Nos teletransportamos a la posición inicial guardada
            transform.position = posDeRespawn;
        }
    }

    // ===== UI AUTOMÁTICA - SE DIBUJA SOLA EN PANTALLA =====
    void OnGUI()
    {
        if (!IsOwner) return;

        GUI.Box(new Rect(10, 10, 220, 80), "=== TÚ ===");
        GUI.Label(new Rect(20, 35, 200, 25), $"  ENERGÍA: {energia.Value}");
        GUI.Label(new Rect(20, 55, 200, 25), $"   VIDAS: {vidas.Value}");
    }
}