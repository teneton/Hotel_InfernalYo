using UnityEngine;

public class LampBehavior : MonoBehaviour
{
    private Light lampLight;
    private bool encendida = false;
    private float intensidadObjetivo = 300f; // Intensidad final al encender
    private float velocidadEncendido = 300f; // Velocidad del efecto de encendido

    // NUEVO: Guardar intensidad inicial
    private float intensidadInicial = 0f;

    void Start()
    {
        lampLight = GetComponent<Light>();
        if (lampLight != null)
        {
            intensidadInicial = lampLight.intensity;
            lampLight.intensity = 0f; // Empieza apagada
        }
    }

    public bool EstaEncendida => encendida;

    public void Encender()
    {
        if (!encendida)
        {
            encendida = true;
        }
    }

    void Update()
    {
        if (encendida && lampLight != null)
        {
            // Efecto de encendido gradual
            if (lampLight.intensity < intensidadObjetivo)
            {
                lampLight.intensity += velocidadEncendido * Time.deltaTime;
                if (lampLight.intensity > intensidadObjetivo)
                    lampLight.intensity = intensidadObjetivo;
            }
        }
    }

    // NUEVO MÉTODO: Resetear lámpara a estado inicial
    public void ResetLamp()
    {
        encendida = false;

        if (lampLight != null)
        {
            lampLight.intensity = 0f; // Volver a apagada
        }

        Debug.Log($" Lámpara {gameObject.name} reseteada");
    }
}
