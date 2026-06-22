using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;
    public float groundDrag = 5f;

    [Header("Colisión")]
    public float collisionDistance = 0.5f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // ¡CRÍTICO! Solo el DUEÑO de este jugador puede moverlo
        if (!IsOwner) return;

        // Detectar si está en el suelo
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f);

        // Capturar entrada WASD
        float moveInput = Input.GetAxis("Vertical");   // W/S o Flechas arriba/abajo
        float turnInput = Input.GetAxis("Horizontal");  // A/D o Flechas izquierda/derecha

        // Calcular dirección relativa al jugador
        moveDirection = transform.forward * moveInput + transform.right * turnInput;
        if (moveDirection.magnitude > 1f) moveDirection.Normalize();

        // Aplicar fricción
        if (rb != null)
        {
            rb.linearDamping = isGrounded ? groundDrag : 0;
        }

        // Rotación (girar siempre que presiones teclas)
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            float turn = turnInput * rotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, turn, 0);
        }

        // IMPORTANTE: Sincronizar movimiento con el servidor
        if (IsServer)
        {
            // El Host mueve localmente
            MoverLocalmente();
        }
        else
        {
            // El Cliente pide al servidor que lo mueva
            RequestMoveServerRpc(moveDirection);
        }
    }

    private void MoverLocalmente()
    {
        if (rb == null || moveDirection.magnitude < 0.01f) return;

        Vector3 movimiento = moveDirection * moveSpeed * Time.deltaTime;

        // Verificar colisión antes de moverse
        if (!HayColision(movimiento))
        {
            rb.MovePosition(rb.position + movimiento);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    private void RequestMoveServerRpc(Vector3 direccion)
    {
        if (!IsServer) return;

        moveDirection = direccion;
        MoverLocalmente();

        // Sincronizar a todos los clientes
        SincronizarPosicionClientRpc(transform.position, transform.rotation);
    }

    [ClientRpc]
    private void SincronizarPosicionClientRpc(Vector3 posicion, Quaternion rotacion)
    {
        // Los clientes ven la posición actualizada
        if (!IsOwner)
        {
            transform.position = posicion;
            transform.rotation = rotacion;
        }
    }

    private bool HayColision(Vector3 movimiento)
    {
        Vector3 dirNormalizada = movimiento.normalized;
        return Physics.Raycast(transform.position, dirNormalizada, collisionDistance);
    }
}