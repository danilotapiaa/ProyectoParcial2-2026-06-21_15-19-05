using Unity.Netcode;
using UnityEngine;

public class DungeonManager : NetworkBehaviour
{
    public NetworkVariable<int> llavesRecogidas = new NetworkVariable<int>(0);
    public static DungeonManager Instance { get; private set; }

    public GameObject puertaCerrada;
    public GameObject puertaAbierta;
    public GameObject zonaVictoria;
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
        Debug.Log($"🔑 Llave recogida! Total: {llavesRecogidas.Value}/5");

        if (llavesRecogidas.Value >= 5)
        {
            AbrirPuertaClientRpc();
        }
    }

    [ClientRpc]
    private void AbrirPuertaClientRpc()
    {
        Debug.Log("🚪 ¡La puerta se abrió!");
        if (puertaCerrada != null) puertaCerrada.SetActive(false);
        if (puertaAbierta != null) puertaAbierta.SetActive(true);
        if (zonaVictoria != null) zonaVictoria.SetActive(true);
    }

    public void DeclararVictoria()
    {
        if (!IsServer) return;
        MostrarVictoriaClientRpc();
    }

    [ClientRpc]
    private void MostrarVictoriaClientRpc()
    {
        Debug.Log("🎉 ¡VICTORIA! ¡Escapaste del laberinto!");
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ===== UI ESTADO DEL JUEGO =====
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 100, 10, 200, 60), "=== ESTADO ===");
        GUI.Label(new Rect(Screen.width / 2 - 90, 30, 180, 25), $"🔑 Llaves: {llavesRecogidas.Value} / 5");
    }
}