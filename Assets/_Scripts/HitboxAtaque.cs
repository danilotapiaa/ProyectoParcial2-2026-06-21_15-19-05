using UnityEngine;
using Unity.Netcode;

public class HitboxAtaque : MonoBehaviour
{
    public float cooldown = 1.5f;
    private float tiempoSiguienteGolpe = 0f;

    private void OnTriggerStay(Collider other)
    {
        // CORRECCIÓN: Primero verificamos si el NetworkManager existe para evitar el NullReferenceException
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer) return;

        if (other.CompareTag("Player") && Time.time > tiempoSiguienteGolpe)
        {
            ControladorJugadorRed salud = other.GetComponent<ControladorJugadorRed>();
            if (salud != null)
            {
                salud.RecibirDanio(20);
                tiempoSiguienteGolpe = Time.time + cooldown;
            }
        }
    }
}