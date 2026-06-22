using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : NetworkBehaviour
{
    private NavMeshAgent agente;
    private Animator anim;
    public float rangoAtaque = 2.5f;

    void Start()
    {
        // Auto-asignación para evitar errores de referencia
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsServer) return;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            agente.SetDestination(jugador.transform.position);
            float dist = Vector3.Distance(transform.position, jugador.transform.position);

            // Animación de correr
            anim.SetFloat("Speed", agente.velocity.magnitude);

            // Ataque
            if (dist <= rangoAtaque)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }
}