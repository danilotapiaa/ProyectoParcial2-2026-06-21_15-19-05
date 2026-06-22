using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : NetworkBehaviour
{
    [Header("Configuración")]
    public NavMeshAgent agente;
    public Animator anim;
    public float rangoAtaque = 2.5f;

    // Usamos el enum para controlar el estado
    private enum Estado { Patrullando, Persiguiendo }
    private Estado estadoActual = Estado.Patrullando;

    private Transform jugadorObjetivo;

    void Start()
    {
        if (agente == null) agente = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsServer) return;

        // Detectar jugador
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        jugadorObjetivo = null;
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                jugadorObjetivo = hit.transform;
                break;
            }
        }

        // Lógica de estados usando la variable
        if (jugadorObjetivo != null)
        {
            estadoActual = Estado.Persiguiendo; // ESTO USA LA VARIABLE
            agente.SetDestination(jugadorObjetivo.position);

            // Animación
            anim.SetFloat("Speed", agente.velocity.magnitude);

            if (Vector3.Distance(transform.position, jugadorObjetivo.position) <= rangoAtaque)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            estadoActual = Estado.Patrullando; // ESTO USA LA VARIABLE
            anim.SetFloat("Speed", agente.velocity.magnitude);
        }

        // Solo para evitar que el compilador diga que no se usa:
        // Debug.Log("Estado actual: " + estadoActual); 
    }
}