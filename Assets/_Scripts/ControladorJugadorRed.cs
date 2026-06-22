using Unity.Netcode;
using UnityEngine;

public class ControladorJugadorRed : NetworkBehaviour
{
    // Variables de Red
    public NetworkVariable<int> energia = new NetworkVariable<int>(100);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3);

    // Variables de BD
    private DataBaseManager dbManager;
    private string jugadorId;
    public string jugadorNombre = "Jugador";

    void Start()
    {
        dbManager = DataBaseManager.Instancia;
        if (IsOwner)
        {
            CargarOCrearId();
            CargarPartida();
        }
    }

    void CargarOCrearId()
    {
        if (PlayerPrefs.HasKey("JugadorId")) jugadorId = PlayerPrefs.GetString("JugadorId");
        else
        {
            jugadorId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("JugadorId", jugadorId);
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (!IsServer) return;

        energia.Value -= cantidad;

        // Lógica de Respawn
        if (energia.Value <= 0)
        {
            vidas.Value--;
            if (vidas.Value > 0)
            {
                energia.Value = 100;
                // Respawn en las coordenadas que solicitaste
                transform.position = new Vector3(-25f, 0.1f, 50f);
            }
            else
            {
                Debug.Log("Game Over: Sin vidas");
                GetComponent<NetworkObject>().Despawn();
            }
        }

        GuardarPartida("Daño recibido");
    }

    void GuardarPartida(string motivo)
    {
        if (dbManager == null) return;
        GameData datos = new GameData
        {
            jugador_id = jugadorId,
            jugador_nombre = jugadorNombre,
            vida = energia.Value,
            posicion_x = transform.position.x,
            posicion_z = transform.position.z,
            nivel = 1
        };
        dbManager.GuardarPartida(datos);
    }

    void CargarPartida()
    {
        StartCoroutine(dbManager.CargarPartida(jugadorId, (datos) => {
            if (datos != null)
            {
                energia.Value = datos.vida;
                transform.position = new Vector3(datos.posicion_x, transform.position.y, datos.posicion_z);
            }
        }));
    }

    void OnGUI()
    {
        if (!IsOwner) return;

        GUI.Box(new Rect(10, 10, 200, 85), "=== ESTADO JUGADOR ===");
        // Etiquetas corregidas
        GUI.Label(new Rect(20, 35, 180, 25), $" ENERGIA: {energia.Value}");
        GUI.Label(new Rect(20, 55, 180, 25), $" VIDAS: {vidas.Value}");
    }
}