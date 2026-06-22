using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 360.0f;

    [Header("Colisiones")]
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private float inputXRaw = 0f;
    private float inputYRaw = 0f;
    private Vector3 direccionMovimientoActual = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ¡CRÍTICO! Solo el DUEÑO de este jugador puede moverlo
        if (!IsOwner) return;

        // Capturar entrada EXACTAMENTE como en movimiento.cs
        Vector2 entradaTeclado = Vector2.zero;
        var teclado = Keyboard.current;

        if (teclado != null)
        {
            if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) entradaTeclado.y = 1f;
            if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) entradaTeclado.y = -1f;
            if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) entradaTeclado.x = 1f;
            if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) entradaTeclado.x = -1f;
        }

        Vector3 direccionMovimiento = new Vector3(entradaTeclado.x, 0.0f, entradaTeclado.y);
        if (direccionMovimiento.sqrMagnitude > 1f) direccionMovimiento.Normalize();

        inputXRaw = entradaTeclado.x;
        inputYRaw = entradaTeclado.y;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        direccionMovimientoActual = forward * inputYRaw + right * inputXRaw;
        if (direccionMovimientoActual.sqrMagnitude > 1f) direccionMovimientoActual.Normalize();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (direccionMovimientoActual.sqrMagnitude > 0.000001f)
        {
            float fixedDt = Time.fixedDeltaTime;
            Vector3 desplazamiento = direccionMovimientoActual * velocidadMovimiento * fixedDt;

            if (rb != null)
            {
                rb.MovePosition(rb.position + desplazamiento);

                if (inputYRaw >= 0)
                {
                    Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimientoActual);
                    rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, rotacionObjetivo, velocidadRotacion * fixedDt));
                }
                else if (Mathf.Abs(inputXRaw) > 0.01f)
                {
                    float giro = inputXRaw * velocidadRotacion * fixedDt;
                    rb.MoveRotation(rb.rotation * Quaternion.Euler(0, giro, 0));
                }
            }
        }
    }
}