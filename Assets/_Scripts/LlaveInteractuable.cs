using Unity.Netcode;
using UnityEngine;

public class LlaveInteractuable : NetworkBehaviour
{
    // Esta bandera evita el doble toque
    private bool yaRecogida = false;

    private void OnTriggerEnter(Collider other)
    {
        // Si no somos el servidor, o si la llave YA se recogió, ignoramos el toque
        if (!IsServer || yaRecogida) return;

        if (other.CompareTag("Player"))
        {
            yaRecogida = true; // Bloqueamos la llave para que no se recoja dos veces
            Debug.Log("¡Llave recogida en el servidor correctamente!");

            // Usamos Despawn(false) para quitarla de la red sin destruirla, quitando la advertencia amarilla
            GetComponent<NetworkObject>().Despawn(false);

            // La apagamos visualmente para que desaparezca de la escena
            gameObject.SetActive(false);
        }
    }
}