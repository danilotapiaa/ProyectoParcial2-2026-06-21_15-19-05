using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject jugadorPrefab;

    private Vector3[] posicionesSpawn = new Vector3[]
    {
        new Vector3(-25f, 0.1f, 50f),  // Posición para Host
        new Vector3(-20f, 0.1f, 50f),  // Posición para Cliente 1
        new Vector3(-15f, 0.1f, 50f)   // Posición para Cliente 2
    };

    private int clientesConectados = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // PRIMERO: Spawnear al Host inmediatamente
        SpawnJugadorLocal(NetworkManager.ServerClientId);

        // LUEGO: Escuchar cuando se conecten clientes
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnJugadorRemoto;
    }

    private void SpawnJugadorLocal(ulong clientId)
    {
        if (jugadorPrefab == null)
        {
            Debug.LogError("PlayerSpawner: jugadorPrefab no está asignado!");
            return;
        }

        Vector3 posicion = posicionesSpawn[Mathf.Min(clientesConectados, posicionesSpawn.Length - 1)];

        GameObject nuevoJugador = Instantiate(jugadorPrefab, posicion, Quaternion.identity);
        NetworkObject networkObject = nuevoJugador.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("PlayerSpawner: El prefab no tiene NetworkObject!");
            return;
        }

        networkObject.SpawnAsPlayerObject(clientId, true);
        clientesConectados++;

        Debug.Log($"✅ HOST spawneado en posición {posicion}");
    }

    private void SpawnJugadorRemoto(ulong clientId)
    {
        if (jugadorPrefab == null)
        {
            Debug.LogError("PlayerSpawner: jugadorPrefab no está asignado!");
            return;
        }

        // NO spawnear al servidor de nuevo
        if (clientId == NetworkManager.ServerClientId)
            return;

        Vector3 posicion = posicionesSpawn[Mathf.Min(clientesConectados, posicionesSpawn.Length - 1)];

        GameObject nuevoJugador = Instantiate(jugadorPrefab, posicion, Quaternion.identity);
        NetworkObject networkObject = nuevoJugador.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("PlayerSpawner: El prefab no tiene NetworkObject!");
            return;
        }

        networkObject.SpawnAsPlayerObject(clientId, true);
        clientesConectados++;

        Debug.Log($"✅ CLIENTE {clientId} spawneado en posición {posicion}");
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnJugadorRemoto;
        }
        base.OnNetworkDespawn();
    }
}