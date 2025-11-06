using UnityEngine;
using UnityEngine.AI;

public class DemonBehaviour : MonoBehaviour
{
    public Transform jugador;
    public GameOverUITMP interfazGameOver;

    public float velocidadNormal = 5.5f;
    public float velocidadMatar = 8f;
    public float distanciaMinima = 6f;

    public float tiempoEntreMovimientos = 0.5f;

    // Área de patrulla libre
    public float minX = -150f, maxX = 150f;
    public float minZ = -150f, maxZ = 150f;

    private NavMeshAgent agente;
    private bool enfadado = false;
    private bool faseFinal = false;
    private float tiempoMovimiento = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima;
        MoverAleatoriamente();
    }

    void Update()
    {
        if (!enfadado && !faseFinal)
        {
            tiempoMovimiento += Time.deltaTime;

            if (tiempoMovimiento >= tiempoEntreMovimientos || agente.remainingDistance < 1f)
            {
                MoverAleatoriamente();
                tiempoMovimiento = 0f;
            }
        }

        if ((enfadado || faseFinal) && jugador != null)
        {
            // Solo actualiza si el destino está lejos
            if (Vector3.Distance(agente.destination, jugador.position) > 1f)
            {
                agente.SetDestination(jugador.position);
            }
        }
    }

    void MoverAleatoriamente()
    {
        Vector3 punto = new Vector3(
            Random.Range(minX, maxX),
            0f,
            Random.Range(minZ, maxZ)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(punto, out hit, 10f, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }

    public void ActivarPersecucionSuave()
    {
        enfadado = true;
        faseFinal = false;
        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima;
        Debug.Log("Demonio en modo persecución suave.");
    }

    public void ActivarModoMatar()
    {
        faseFinal = true;
        agente.speed = velocidadMatar;
        agente.stoppingDistance = 0f;
        Teletransportarse();
        Debug.Log("Demonio en modo matar.");
    }

    public void Calmar()
    {
        if (!enfadado && !faseFinal)
        {
            Debug.Log("Demonio calmado");
            return;
        }

        enfadado = false;
        faseFinal = false;
        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima;
        MoverAleatoriamente();

        Debug.Log("Demonio calmado: estado reiniciado, velocidad normal restaurada.");
    }

    public void Teletransportarse()
    {
        if (jugador != null)
        {
            Vector3 destino = jugador.position + jugador.forward * 1.5f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(destino, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                transform.LookAt(jugador.position);
                agente.SetDestination(jugador.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((enfadado || faseFinal) && other.CompareTag("Player"))
        {
            Debug.Log("Trigger detectado con el jugador.");

            if (interfazGameOver != null)
            {
                interfazGameOver.ShowGameOverMessage();
                Time.timeScale = 0f;
                Debug.Log(" El demonio ha atrapado al jugador.");
            }
            else
            {
                Debug.LogWarning(" Interfaz Game Over no asignada.");
            }
        }
    }
}



