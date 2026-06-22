using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : NetworkBehaviour
{
    public NavMeshAgent agente;
    public Animator anim;
    public float rangoAtaque = 2.5f;

    private enum Estado { Patrullando, Persiguiendo }
    private Estado estadoActual = Estado.Patrullando;

    void Start()
    {
        // Auto-asignación para que no salgan errores de referencia
        if (agente == null) agente = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsServer) return;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            estadoActual = Estado.Persiguiendo;
            agente.SetDestination(jugador.transform.position);
            anim.SetFloat("Speed", agente.velocity.magnitude);

            if (Vector3.Distance(transform.position, jugador.transform.position) <= rangoAtaque)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            estadoActual = Estado.Patrullando;
            anim.SetFloat("Speed", 0);
        }
    }
}