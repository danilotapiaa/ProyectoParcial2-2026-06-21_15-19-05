using Unity.Netcode;
using UnityEngine;

public class SaludJugadorRed : NetworkBehaviour
{
    // Variables sincronizadas. Todos pueden leerlas (Everyone), pero SOLO el Servidor puede modificarlas para evitar trampas.
    public NetworkVariable<int> energia = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> vidas = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void OnGUI()
    {
        // REGLA DE ORO: Solo dibujamos este cuadro si somos el dueño de este personaje en nuestra pantalla.
        if (!IsOwner) return;

        // Dibujamos el HUD nativo en la esquina superior izquierda
        GUI.Box(new Rect(10, 10, 150, 70), "=== TÚ ===");
        GUI.Label(new Rect(20, 35, 130, 20), "Energía: " + energia.Value + "%");
        GUI.Label(new Rect(20, 55, 130, 20), "Vidas: " + vidas.Value);
    }

    // Método listo para cuando los monstruos regresen y te ataquen
    public void RecibirDanio(int cantidad)
    {
        // Solo el servidor tiene autoridad para restar vida
        if (!IsServer) return;

        energia.Value -= cantidad;

        if (energia.Value <= 0)
        {
            vidas.Value -= 1;
            energia.Value = 100; // Restaurar energía al perder una vida

            // Más adelante aquí pondremos el código para teletransportarte de regreso al inicio si mueres
        }
    }
}