using Unity.Netcode;
using UnityEngine;

public class DungeonManager : NetworkBehaviour
{
    public NetworkVariable<int> llavesRecogidas = new NetworkVariable<int>(0);
    public static DungeonManager Instance { get; private set; }

    // Variables para los dos modelos de la puerta
    public GameObject puertaCerrada;
    public GameObject puertaAbierta;

    public GameObject zonaVictoria;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegistrarLlave()
    {
        if (!IsServer) return;

        llavesRecogidas.Value++;
        Debug.Log($"¡Llaves actuales: {llavesRecogidas.Value} / 5!");

        if (llavesRecogidas.Value >= 5)
        {
            // El servidor ejecuta la orden para que todos los clientes abran la puerta
            AbrirPuertaClientRpc();
        }
    }

    // Este decorador indica que la función la pide el Servidor, pero se ejecuta en los Clientes
    [ClientRpc]
    private void AbrirPuertaClientRpc()
    {
        Debug.Log("¡Intercambiando estado de la puerta para todos los jugadores!");

        // Apagamos el modelo cerrado y encendemos el modelo abierto
        if (puertaCerrada != null) puertaCerrada.SetActive(false);
        if (puertaAbierta != null) puertaAbierta.SetActive(true);

        // Activamos la zona de victoria
        if (zonaVictoria != null) zonaVictoria.SetActive(true);
    }
}