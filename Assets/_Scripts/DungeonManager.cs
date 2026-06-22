using Unity.Netcode;
using UnityEngine;

public class DungeonManager : NetworkBehaviour
{
    public NetworkVariable<int> llavesRecogidas = new NetworkVariable<int>(0);
    public static DungeonManager Instance { get; private set; }

    public GameObject puertaCerrada;
    public GameObject puertaAbierta;
    public GameObject zonaVictoria;

    // Nueva variable para la interfaz
    public GameObject panelVictoria;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegistrarLlave()
    {
        if (!IsServer) return;

        llavesRecogidas.Value++;

        if (llavesRecogidas.Value >= 5)
        {
            AbrirPuertaClientRpc();
        }
    }

    [ClientRpc]
    private void AbrirPuertaClientRpc()
    {
        if (puertaCerrada != null) puertaCerrada.SetActive(false);
        if (puertaAbierta != null) puertaAbierta.SetActive(true);
        if (zonaVictoria != null) zonaVictoria.SetActive(true);
    }

    // --- NUEVA LÓGICA DE VICTORIA ---
    public void DeclararVictoria()
    {
        if (!IsServer) return; // Solo el servidor valida la meta
        MostrarVictoriaClientRpc(); // Le avisa a todos los clientes que ganaron
    }

    [ClientRpc]
    private void MostrarVictoriaClientRpc()
    {
        Debug.Log("¡Mostrando panel de victoria a los jugadores!");
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);

            // Opcional: Desbloquear el cursor del mouse para que puedan salir del juego
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}