using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : NetworkBehaviour
{
    private NavMeshAgent agente;
    private Animator anim;
    public float rangoAtaque = 2.5f;

    // NUEVO: Variables para evitar que convulsione al atacar
    public float cooldownAtaque = 1.5f;
    private float tiempoParaSiguienteAtaque = 0f;

    public NetworkVariable<float> velocidadRed = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Todos los clientes sincronizan la animación
        anim.SetFloat("Speed", velocidadRed.Value);

        if (!IsServer) return;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            agente.SetDestination(jugador.transform.position);
            float dist = Vector3.Distance(transform.position, jugador.transform.position);

            velocidadRed.Value = agente.velocity.magnitude;

            // NUEVO: Solo ataca si ya pasó su cooldown
            if (dist <= rangoAtaque && Time.time >= tiempoParaSiguienteAtaque)
            {
                tiempoParaSiguienteAtaque = Time.time + cooldownAtaque; // Reinicia el cronómetro
                AtacarClientRpc();
            }
        }
        else
        {
            velocidadRed.Value = 0f;
        }
    }

    [ClientRpc]
    private void AtacarClientRpc()
    {
        anim.SetTrigger("Attack");
    }
}