using UnityEngine;
using UnityEngine.AI;

public class DemonBehaviour : MonoBehaviour
{
    public Transform jugador;
    public GameOverUITMP interfazGameOver;
    public Transform[] puntosPatrulla;
    
    public float velocidadNormal = 5.5f;
    public float velocidadMatar = 8f;
    public float distanciaMinima = 6f;

    private NavMeshAgent agente;
    private bool enfadado = false;
    private bool faseFinal = false;
    private int puntoActual = -1;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima;

        MoverAPuntoDePatrulla();
    }

    void Update()
    {
        // Mientras está tranquilo y no en modo matar, patrulla
        if (!enfadado && !faseFinal)
        {
            if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
            {
                MoverAPuntoDePatrulla();
            }
        }

        // Si está enfadado o en fase final, persigue al jugador
        if ((enfadado || faseFinal) && jugador != null)
        {
            // ACTUALIZACIÓN: Siempre actualizar destino hacia el jugador
            agente.SetDestination(jugador.position);
            
            // DEBUG: Mostrar distancia al jugador
            if (Time.frameCount % 120 == 0) // Cada ~2 segundos
            {
                float distancia = Vector3.Distance(transform.position, jugador.position);
                Debug.Log($"Demonio persiguiendo - Distancia: {distancia:F1}m, " +
                         $"Enfadado: {enfadado}, FaseFinal: {faseFinal}, " +
                         $"Velocidad: {agente.speed}");
            }
        }
    }

    void MoverAPuntoDePatrulla()
    {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0) return;

        int nuevoIndice;
        do
        {
            nuevoIndice = Random.Range(0, puntosPatrulla.Length);
        } 
        while (nuevoIndice == puntoActual && puntosPatrulla.Length > 1);

        puntoActual = nuevoIndice;
        agente.SetDestination(puntosPatrulla[puntoActual].position);
        Debug.Log("Demonio patrullando hacia: " + puntosPatrulla[puntoActual].name);
    }

    public void ActivarPersecucionSuave()
    {
        enfadado = true;
        faseFinal = false;

        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima; // Se mantiene a distancia

        Debug.Log("Demonio en modo persecución suave.");
    }

    public void ActivarModoMatar()
    {
        enfadado = true; // También enfadado en modo matar
        faseFinal = true;

        agente.speed = velocidadMatar;
        agente.stoppingDistance = 0f; // CERO distancia para atrapar

        // Teletransportarse cerca del jugador inmediatamente
        Teletransportarse();

        Debug.Log("MODO MATAR ACTIVADO - Demonio viene a por ti!");
    }

    public void Calmar()
    {
        if (faseFinal)
        {
            Debug.Log("No se puede calmar al demonio en modo matar.");
            return;
        }

        if (!enfadado)
        {
            Debug.Log("El demonio ya está calmado.");
            return;
        }

        enfadado = false;
        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaMinima;

        MoverAPuntoDePatrulla();

        Debug.Log("Demonio calmado: vuelve a patrullar.");
    }

    public void Teletransportarse()
    {
        if (jugador != null)
        {
            // Busca una posición a unos 10-15 metros del jugador (no tan cerca)
            Vector3 direccionAleatoria = Random.onUnitSphere;
            direccionAleatoria.y = 0; // Mantener en el mismo plano Y
            direccionAleatoria.Normalize();
            
            Vector3 destino = jugador.position + direccionAleatoria * 12f;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(destino, out hit, 10f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                agente.SetDestination(jugador.position);
                
                Debug.Log($"Demonio teletransportado a {Vector3.Distance(transform.position, jugador.position):F1}m del jugador");
            }
            else
            {
                // Fallback: posición más cercana disponible
                if (NavMesh.SamplePosition(jugador.position, out hit, 20f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                    agente.SetDestination(jugador.position);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (faseFinal && other.CompareTag("Player"))
        {
            Debug.Log("¡Demonio atrapó al jugador!");

            if (interfazGameOver != null)
            {
                interfazGameOver.ShowGameOverMessage();
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogWarning("Interfaz Game Over no asignada.");
            }
        }
    }
}