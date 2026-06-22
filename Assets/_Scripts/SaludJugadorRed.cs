using Unity.Netcode;
using UnityEngine;

public class SaludJugadorRed : NetworkBehaviour
{
    public NetworkVariable<int> energia = new NetworkVariable<int>(100);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3);

    public void RecibirDanio(int cantidad)
    {
        if (!IsServer) return;

        energia.Value -= cantidad;

        if (energia.Value <= 0)
        {
            PerderVida();
        }
    }

    private void PerderVida()
    {
        vidas.Value--;
        if (vidas.Value <= 0)
        {
            Debug.Log("Juego Terminado");
            GetComponent<NetworkObject>().Despawn();
        }
        else
        {
            energia.Value = 100;
            // Teletransporte al Respawn
            transform.position = new Vector3(-25f, 0.1f, 50f);
        }
    }
}