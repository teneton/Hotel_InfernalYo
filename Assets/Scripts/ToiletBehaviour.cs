using UnityEngine;

public class ToiletBehavior : MonoBehaviour
{
    private bool limpio = false;

    [Header("Objeto visual del agua del váter")]
    public Renderer aguaRenderer; // El objeto que representa el agua (ej. un mesh con material)
    public Material materialLimpio;
    public Material materialSucio;
    public bool EstaLimpio => limpio;

    public void Limpiar()
    {
        if (!limpio)
        {
            limpio = true;

            if (aguaRenderer != null && materialLimpio != null)
            {
                // Cambia el material completo al que quieras
                aguaRenderer.material = materialLimpio;
            }

            Debug.Log($"Váter {gameObject.name} limpiado.");
        }
    }

    void Start()
    {
        // Inicialmente el agua está sucia
        if (aguaRenderer != null && materialSucio != null)
        {
            aguaRenderer.material = materialSucio;
        }
    }

    // NUEVO MÉTODO: Resetear váter a estado sucio
    public void ResetToilet()
    {
        limpio = false;

        if (aguaRenderer != null && materialSucio != null)
        {
            aguaRenderer.material = materialSucio;
        }

        Debug.Log($"🚽 Váter {gameObject.name} reseteado a estado sucio");
    }
}
