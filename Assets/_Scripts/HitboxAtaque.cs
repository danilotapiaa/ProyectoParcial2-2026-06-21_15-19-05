using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    public int daño = 10;
    private float tiempoUltimoAtaque = 0f;
    public float cooldownAtaque = 1.0f; // 1 segundo entre golpes

    private void OnTriggerStay(Collider other) // Usamos Stay para detectar mientras toca
    {
        if (other.CompareTag("Player") && Time.time > tiempoUltimoAtaque)
        {
            SaludJugadorRed salud = other.GetComponent<SaludJugadorRed>();
            if (salud != null)
            {
                salud.RecibirDanio(daño);
                tiempoUltimoAtaque = Time.time + cooldownAtaque;
                Debug.Log("¡Golpeaste al jugador!");
            }
        }
    }
}