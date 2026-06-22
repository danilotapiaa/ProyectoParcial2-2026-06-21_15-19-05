using Unity.Netcode;
using UnityEngine;
using Unity.Cinemachine;

public class AutoAssignCamera : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (vcam != null && vcam.Follow == null)
        {
            // 1. Verificamos que el multijugador esté activo y estemos conectados
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient)
            {
                // 2. MAGIA: Obtenemos ÚNICAMENTE el personaje que nos pertenece a nosotros
                var localPlayerObject = NetworkManager.Singleton.LocalClient.PlayerObject;

                if (localPlayerObject != null)
                {
                    // 3. Buscamos la raíz de la cámara dentro de NUESTRO jugador
                    Transform cameraRoot = localPlayerObject.transform.Find("PlayerCameraRoot");

                    if (cameraRoot != null)
                    {
                        vcam.Follow = cameraRoot;
                        vcam.LookAt = cameraRoot;
                    }
                }
            }
        }
    }
}