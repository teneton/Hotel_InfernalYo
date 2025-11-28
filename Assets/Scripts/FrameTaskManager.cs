using UnityEngine;

public class FrameTaskManager : MonoBehaviour
{
    public Camera playerCamera;                   // Cámara del jugador
    public FrameBehavior[] cuadros;               // Array de cuadros a enderezar
    private FrameBehavior cuadroActual;           // Cuadro que el jugador está mirando
    private bool cerca = false;                   // Si el jugador está cerca de un cuadro
    private bool tareaCompletada = false;         // Si todos los cuadros están rectos
    public PlayerMovement playerMovement;         // Referencia al jugador

    void Update()
    {
        // Si la tarea está completada, no hacer nada
        if (tareaCompletada) return;

        // Bloquear interacción si el jugador lleva un objeto
        if (playerMovement != null && playerMovement.EstaLlevandoObjeto)
        {
            cerca = false;
            cuadroActual = null;
            return;
        }

        // Detectar cuadro con raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f))
        {
            FrameBehavior frame = hit.collider.GetComponent<FrameBehavior>();
            if (frame != null && !frame.EstaRecto)
            {
                cuadroActual = frame;
                cerca = true;
            }
            else
            {
                cerca = false;
                cuadroActual = null;
            }
        }
        else
        {
            cerca = false;
            cuadroActual = null;
        }

        // Interacción simple con una pulsación de E
        if (cerca && Input.GetKeyDown(KeyCode.E) && cuadroActual != null)
        {
            cuadroActual.Enderezar();
        }

        // Verificar si todos los cuadros están rectos
        if (TodosRectos())
        {
            tareaCompletada = true;
            Debug.Log("¡Todos los cuadros están rectos! Tarea completada.");
        }
    }

    // Verifica si todos los cuadros están rectos
    bool TodosRectos()
    {
        foreach (FrameBehavior frame in cuadros)
        {
            if (!frame.EstaRecto) return false;
        }
        return true;
    }

    // Método para verificar si la tarea está completada
    public bool TareaCompletada()
    {
        return tareaCompletada;
    }

    // 🔄 NUEVO MÉTODO: Resetear la tarea de cuadros
    public void ResetTask()
    {
        Debug.Log("🔄 Reseteando tarea de cuadros...");

        tareaCompletada = false;
        cerca = false;
        cuadroActual = null;

        // Resetear cada cuadro individual
        foreach (FrameBehavior frame in cuadros)
        {
            if (frame != null)
            {
                frame.ResetFrame();
            }
        }

        Debug.Log("✅ Tarea de cuadros reseteada");
    }

    // Mostrar mensaje de interacción en pantalla
    void OnGUI()
    {
        if (cerca && cuadroActual != null && !cuadroActual.EstaRecto)
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
