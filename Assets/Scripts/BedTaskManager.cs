using UnityEngine;

public class BedTaskManager : MonoBehaviour
{
    public Camera playerCamera;                   // Cámara del jugador
    public BedObjectBehavior[] objetosCama;       // Array de objetos de la cama
    private BedObjectBehavior objetoActual;       // Objeto que el jugador está mirando
    private bool cerca = false;                   // Si el jugador está cerca de un objeto
    private bool tareaCompletada = false;         // Si todos los objetos están completados

    private float tiempoMantener = 3f;            // Tiempo necesario para interactuar
    private float contadorMantener = 0f;          // Contador de tiempo manteniendo E
    private bool manteniendo = false;             // Si se está manteniendo E

    public PlayerMovement playerMovement;         // Referencia al jugador

    // Propiedad pública para que GameTaskManager pueda acceder
    public bool TareaCompletada()
    {
        return tareaCompletada;
    }

    void Update()
    {
        // Si la tarea está completada, no hacer nada
        if (tareaCompletada) return;

        // Bloquear interacción si el jugador lleva un objeto
        if (playerMovement != null && playerMovement.EstaLlevandoObjeto)
        {
            cerca = false;
            objetoActual = null;
            return;
        }

        // Detectar objeto con raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f))
        {
            BedObjectBehavior obj = hit.collider.GetComponent<BedObjectBehavior>();
            if (obj != null && !obj.EstaCompletado)
            {
                objetoActual = obj;
                cerca = true;
            }
            else
            {
                cerca = false;
                objetoActual = null;
            }
        }
        else
        {
            cerca = false;
            objetoActual = null;
        }

        // Interacción manteniendo E
        if (cerca && objetoActual != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                manteniendo = true;
                contadorMantener += Time.deltaTime;

                // Cuando se completa el tiempo, interactuar con el objeto
                if (contadorMantener >= tiempoMantener)
                {
                    objetoActual.Interactuar();
                    contadorMantener = 0f;
                    manteniendo = false;

                    // Verificar si todos los objetos están completados
                    if (TodosCompletados())
                    {
                        tareaCompletada = true;
                        Debug.Log("✅ ¡Todos los objetos de la cama han sido rotados y movidos! Tarea completada.");
                    }
                }
            }
            else
            {
                manteniendo = false;
                contadorMantener = 0f;
            }
        }
    }

    // Verifica si todos los objetos están completados
    bool TodosCompletados()
    {
        foreach (BedObjectBehavior obj in objetosCama)
        {
            if (!obj.EstaCompletado)
                return false;
        }
        return true;
    }

    // 🔄 NUEVO MÉTODO: Resetear la tarea de la cama
    public void ResetTask()
    {
        Debug.Log("🔄 Reseteando tarea de la cama...");

        tareaCompletada = false;
        cerca = false;
        objetoActual = null;
        manteniendo = false;
        contadorMantener = 0f;

        // Resetear cada objeto de cama individual
        foreach (BedObjectBehavior obj in objetosCama)
        {
            if (obj != null)
            {
                obj.ResetObject();
            }
        }

        Debug.Log("✅ Tarea de la cama reseteada");
    }

    // Mostrar mensaje de interacción en pantalla
    void OnGUI()
    {
        if (cerca && objetoActual != null && !objetoActual.EstaCompletado)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.label);
            estilo.fontSize = 40;
            estilo.normal.textColor = Color.white;
            estilo.alignment = TextAnchor.MiddleCenter;

            Rect mensaje = new Rect(Screen.width / 2 - 200, Screen.height - 120, 400, 80);

            if (manteniendo)
            {
                // Mostrar progreso de interacción
                float progreso = contadorMantener / tiempoMantener;
                GUI.Label(mensaje, $"Haciendo cama... {progreso * 100:F0}%", estilo);
            }
            else
            {
                string texto = objetoActual != null && objetoActual.EstaCompletado ? "" : "Mantén E para interactuar";
                if (!string.IsNullOrEmpty(texto))
                    GUI.Label(mensaje, texto, estilo);
            }
        }
    }
}
