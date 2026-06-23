using UnityEngine;
using Unity.Netcode;
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
        if (vcam == null) return;

        if (vcam.Follow != null) return;

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient)
        {
            var jugadorLocal = NetworkManager.Singleton.LocalClient.PlayerObject;

            if (jugadorLocal != null)
            {
                Transform puntoCamara = jugadorLocal.transform.Find("PlayerCameraRoot");

                if (puntoCamara != null)
                {
                    vcam.Follow = puntoCamara;

                    // ESTA ES LA MAGIA: Le dice a la cámara que se teletransporte de golpe, sin animaciones chistosas
                    vcam.PreviousStateIsValid = false;
                }
            }
        }
    }
}