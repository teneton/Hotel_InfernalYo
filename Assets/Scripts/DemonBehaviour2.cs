using UnityEngine;
using UnityEngine.AI;

public class DemonBehaviour2 : MonoBehaviour
{
    public Transform jugador;
    public float velocidadRotacion = 3f;
    public GameOverUITMP interfazGameOver;

    private NavMeshAgent agente;
    private bool enfadado = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente != null)
        {
            agente.isStopped = true;
            agente.stoppingDistance = 0f;   
            agente.autoBraking = false;     
            agente.updateRotation = true;   
        }
    }

    void Update()
    {
        if (jugador != null)
        {
            Vector3 direccion = jugador.position - transform.position;
            direccion.y = 0;

            if (direccion != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            }

            if (enfadado && agente != null)
            {
                agente.isStopped = false;
                agente.SetDestination(jugador.position);
            }
        }
    }

    public void ActivarPersecucionRapida()
    {
        if (agente != null && jugador != null)
        {
            agente.speed = 25f;
            enfadado = true;
            Debug.Log("Demonio2 enfadado: comienza persecución rápida continua.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (enfadado && other.CompareTag("Player"))
        {
            Debug.Log("Demonio2 ha atrapado al jugador.");
            if (interfazGameOver != null)
            {
                interfazGameOver.ShowGameOverMessage();
                Time.timeScale = 0f;
            }
        }
    }
}

