using UnityEngine;

public class FaucetBehavior : MonoBehaviour
{
    private bool cerrado = false;
    public Renderer rend;

    [Header("Objeto visual del agua")]
    public GameObject aguaVisual; // ← El objeto que representa el agua

    // 🔄 NUEVO: Guardar estado inicial
    private Material materialInicial;
    private Color colorInicial;

    void Start()
    {
        // Guardar estado inicial
        if (rend != null)
        {
            materialInicial = rend.material;
            colorInicial = rend.material.color;
        }

        if (aguaVisual != null)
            aguaVisual.SetActive(true); // Empieza con agua activa
    }

    public bool EstaCerrado => cerrado;

    public void Cerrar()
    {
        if (!cerrado)
        {
            cerrado = true;

            if (rend != null)
                rend.material.color = Color.gray; // Color al cerrar

            if (aguaVisual != null)
                aguaVisual.SetActive(false); // Oculta el agua

            Debug.Log($"Grifo {gameObject.name} cerrado.");
        }
    }

    public void Abrir()
    {
        if (cerrado)
        {
            cerrado = false;

            if (rend != null)
                rend.material.color = Color.blue; // Color al abrir

            if (aguaVisual != null)
                aguaVisual.SetActive(true); // Muestra el agua

            Debug.Log($"Grifo {gameObject.name} abierto.");
        }
    }

    // 🔄 NUEVO MÉTODO: Resetear grifo a estado inicial
    public void ResetFaucet()
    {
        cerrado = false;

        // Restaurar material y color inicial
        if (rend != null)
        {
            rend.material = materialInicial;
            rend.material.color = colorInicial;
        }

        // Reactivar agua visual
        if (aguaVisual != null)
        {
            aguaVisual.SetActive(true);
        }

        Debug.Log($"🚰 Grifo {gameObject.name} reseteado");
    }
}