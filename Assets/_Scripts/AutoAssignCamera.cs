using UnityEngine;
using Unity.Cinemachine;

public class AutoAssignCamera : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Start()
    {
        // Actualizado para usar el nuevo nombre de componente en Cinemachine 3+
        vcam = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (vcam != null && vcam.Follow == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Transform cameraRoot = player.transform.Find("PlayerCameraRoot");

                if (cameraRoot != null)
                {
                    vcam.Follow = cameraRoot;
                    vcam.LookAt = cameraRoot;
                }
            }
        }
    }
}