using UnityEngine;

public class ControlLucesEscenario : MonoBehaviour
{
    public Light[] luces; // arrastra todas las luces de la escena aquí
    public float intensidadAlta = 1.5f;
    public float intensidadBaja = 0.3f;

    void Start()
    {
        bool encendidas = GameManager.instancia.lucesEncendidas;

        foreach (Light luz in luces)
        {
            luz.intensity = encendidas ? intensidadAlta : intensidadBaja;
        }
    }
}

