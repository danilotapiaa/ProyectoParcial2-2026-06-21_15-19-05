using Unity.Netcode;
using UnityEngine;

public class ZonaVictoria : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo el Host registra oficialmente quién cruzó la meta
        if (!IsServer) return;

        // Si el que tocó la zona es un jugador...
        if (other.CompareTag("Player"))
        {
            // Le avisamos al Gestor que alguien logró escapar
            if (DungeonManager.Instance != null)
            {
                DungeonManager.Instance.DeclararVictoria();
            }

            // Apagamos la zona para que no lance el evento mil veces
            gameObject.SetActive(false);
        }
    }
}