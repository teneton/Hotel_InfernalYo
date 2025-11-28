using UnityEngine;

public class LampTaskManager : MonoBehaviour
{
    public Camera playerCamera;                   // Cámara del jugador
    public LampBehavior[] lamparas;               // Array de lámparas a encender
    private LampBehavior lamparaActual;           // Lámpara que el jugador está mirando
    private bool cerca = false;                   // Si el jugador está cerca de una lámpara
    private bool tareaCompletada = false;         // Si todas las lámparas están encendidas
    public PlayerMovement playerMovement;         // Referencia al jugador

    void Update()
    {
        // Si la tarea está completada, no hacer nada
        if (tareaCompletada) return;

        // Bloquear interacción si el jugador lleva un objeto
        if (playerMovement != null && playerMovement.EstaLlevandoObjeto)
        {
            cerca = false;
            lamparaActual = null;
            return;
        }

        // Detectar lámpara con raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f))
        {
            LampBehavior lamp = hit.collider.GetComponent<LampBehavior>();
            if (lamp != null && !lamp.EstaEncendida)
            {
                lamparaActual = lamp;
                cerca = true;
            }
            else
            {
                cerca = false;
                lamparaActual = null;
            }
        }
        else
        {
            cerca = false;
            lamparaActual = null;
        }

        // Interacción simple con una pulsación de E
        if (cerca && Input.GetKeyDown(KeyCode.E) && lamparaActual != null)
        {
            lamparaActual.Encender();
            Debug.Log($"Lámpara {lamparaActual.name} encendida.");

            // Verificar si todas las lámparas están encendidas
            if (TodasEncendidas())
            {
                tareaCompletada = true;
                Debug.Log("✅ ¡Todas las lámparas encendidas! Tarea completada.");
            }
        }
    }

    // Verifica si todas las lámparas están encendidas
    bool TodasEncendidas()
    {
        foreach (LampBehavior lamp in lamparas)
        {
            if (!lamp.EstaEncendida) return false;
        }
        return true;
    }

    // Método para verificar si la tarea está completada
    public bool TareaCompletada()
    {
        return tareaCompletada;
    }

    // 🔄 NUEVO MÉTODO: Resetear la tarea de lámparas
    public void ResetTask()
    {
        Debug.Log("🔄 Reseteando tarea de lámparas...");

        tareaCompletada = false;
        cerca = false;
        lamparaActual = null;

        // Resetear cada lámpara individual
        foreach (LampBehavior lamp in lamparas)
        {
            if (lamp != null)
            {
                lamp.ResetLamp();
            }
        }

        Debug.Log("✅ Tarea de lámparas reseteada");
    }

    // Mostrar mensaje de interacción en pantalla
    void OnGUI()
    {
        if (cerca && lamparaActual != null && !lamparaActual.EstaEncendida)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.label);
            estilo.fontSize = 40;
            estilo.normal.textColor = Color.white;
            estilo.alignment = TextAnchor.MiddleCenter;

            Rect mensaje = new Rect(Screen.width / 2 - 200, Screen.height - 120, 400, 80);
            GUI.Label(mensaje, "E para interactuar", estilo);
        }
    }
}