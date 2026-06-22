using UnityEngine;
using Unity.Netcode;

public class HitboxAtaque : MonoBehaviour
{
    private float cooldown = 1.0f;
    private float tiempoSiguienteGolpe = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time > tiempoSiguienteGolpe)
        {
            ControladorJugadorRed salud = other.GetComponent<ControladorJugadorRed>();
            if (salud != null)
            {
                salud.RecibirDanio(20);
                tiempoSiguienteGolpe = Time.time + cooldown;
                Debug.Log("¡Golpeaste al jugador! Daño aplicado.");
            }
        }
    }
}