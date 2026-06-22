using Unity.Netcode;
using UnityEngine;

public class UIGlobal : MonoBehaviour
{
    // Para que se vea el estado global sin depender del Inspector
    void OnGUI()
    {
        // ===== LLAVES GLOBALES =====
        if (DungeonManager.Instance != null)
        {
            int llaves = DungeonManager.Instance.llavesRecogidas.Value;
            GUI.Box(new Rect(Screen.width - 250, 10, 240, 60), "=== LLAVES ===");
            GUI.Label(new Rect(Screen.width - 240, 30, 220, 25), $"🔑 Recogidas: {llaves} / 5");
        }

        // ===== INFORMACIÓN DE TODOS LOS JUGADORES =====
        ControladorJugadorRed[] jugadores = FindObjectsByType<ControladorJugadorRed>(FindObjectsInactive.Exclude);

        if (jugadores.Length > 0)
        {
            int yOffset = 80;

            foreach (ControladorJugadorRed j in jugadores)
            {
                // Si es el enemigo (no el dueño), mostrar su estado
                if (!j.IsOwner)
                {
                    GUI.Box(new Rect(10, yOffset, 220, 80), "=== ENEMIGO ===");
                    GUI.Label(new Rect(20, yOffset + 25, 200, 25), $"⚡ ENERGÍA: {j.energia.Value}");
                    GUI.Label(new Rect(20, yOffset + 45, 200, 25), $"❤️  VIDAS: {j.vidas.Value}");
                    yOffset += 90;
                }
            }
        }
    }
}