using UnityEngine;

public class InteractionDemon : MonoBehaviour
{
    public float tiempoLimite = 60f;
    public float tiempoFinal = 90f;
    public DemonBehaviour demonio;
    public Renderer cuboRenderer;
    public Camera camaraJugador;
    public float distanciaInteraccion = 2f;

    private float tiempoActual = 0f;
    private bool faseFinal = false;
    private bool jugadorCerca = false;
    private bool cronometroDetenido = false;

    void Start()
    {
        tiempoActual = 0f;

        if (cuboRenderer != null)
            cuboRenderer.material.color = Color.white;

        if (camaraJugador == null)
            Debug.LogError("Asigna la cámara del jugador en el inspector.");
    }

    void Update()
    {
        jugadorCerca = false;

        RaycastHit hit;
        if (camaraJugador != null && Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, distanciaInteraccion))
        {
            if (hit.collider.gameObject == gameObject)
            {
                jugadorCerca = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (cuboRenderer != null)
                        cuboRenderer.material.color = Color.green;

                    if (!faseFinal && demonio != null)
                    {
                        cronometroDetenido = true;
                        demonio.Calmar();
                        Debug.Log("Cubo tocado: demonio calmado y cronómetro detenido.");
                    }
                    else
                    {
                        Debug.Log("Cubo tocado, pero el demonio ya no se calma.");
                    }
                }
            }
        }

        if (!cronometroDetenido && !faseFinal)
        {
            tiempoActual += Time.deltaTime;

            if (tiempoActual >= tiempoLimite && demonio != null)
            {
                demonio.ActivarPersecucionSuave();
            }

            if (tiempoActual >= tiempoFinal && demonio != null)
            {
                faseFinal = true;
                demonio.ActivarModoMatar();
            }
        }
    }

    public bool EstaEnFaseFinal() => faseFinal;
    public bool IsPlayerNear() => jugadorCerca;
    public float GetRemainingTime() => Mathf.Max(0f, tiempoFinal - tiempoActual);
}




