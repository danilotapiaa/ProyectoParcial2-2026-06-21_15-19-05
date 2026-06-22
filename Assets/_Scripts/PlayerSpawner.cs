using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject jugadorPrefab; // Asignar tu prefab de jugador

    private Vector3[] posicionesSpawn = new Vector3[]
    {
        new Vector3(-25f, 0.1f, 50f),  // Posición 1 para Host
        new Vector3(-20f, 0.1f, 50f)   // Posición 2 para Cliente
    };

    private int clientesConectados = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // Cuando se conecta un cliente, spawn un jugador para él
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnJugador;
    }

    private void SpawnJugador(ulong clientId)
    {
        if (jugadorPrefab == null)
        {
            Debug.LogError("PlayerSpawner: jugadorPrefab no está asignado!");
            return;
        }

        Vector3 posicion = posicionesSpawn[Mathf.Min(clientesConectados, 1)];

        GameObject nuevoJugador = Instantiate(jugadorPrefab, posicion, Quaternion.identity);
        NetworkObject networkObject = nuevoJugador.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("PlayerSpawner: El prefab no tiene NetworkObject!");
            return;
        }

        // ¡IMPORTANTE! Spawnearlo en red y asignarlo al cliente específico
        networkObject.SpawnAsPlayerObject(clientId, true);

        clientesConectados++;
        Debug.Log($"✅ Jugador spawneado para cliente {clientId} en posición {posicion}");
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnJugador;
        }
        base.OnNetworkDespawn();
    }
}