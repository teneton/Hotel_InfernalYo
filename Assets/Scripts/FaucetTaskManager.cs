using UnityEngine;

public class FaucetTaskManager : MonoBehaviour
{
    public Camera playerCamera;                   // Cámara del jugador
    public FaucetBehavior[] grifos;               // Array de grifos a cerrar
    private FaucetBehavior grifoActual;           // Grifo que el jugador está mirando
    private bool cerca = false;                   // Si el jugador está cerca de un grifo
    private bool tareaCompletada = false;         // Si todos los grifos están cerrados
    public PlayerMovement playerMovement;         // Referencia al jugador

    void Update()
    {
        // Si la tarea está completada, no hacer nada
        if (tareaCompletada) return;

        // Bloquear interacción si el jugador lleva un objeto
        if (playerMovement != null && playerMovement.EstaLlevandoObjeto)
        {
            cerca = false;
            grifoActual = null;
            return;
        }

        // Detectar grifo con raycast
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f))
        {
            FaucetBehavior faucet = hit.collider.GetComponent<FaucetBehavior>();
            if (faucet != null && !faucet.EstaCerrado)
            {
                grifoActual = faucet;
                cerca = true;
            }
            else
            {
                cerca = false;
                grifoActual = null;
            }
        }
        else
        {
            cerca = false;
            grifoActual = null;
        }

        // Interacción simple con una pulsación de E
        if (cerca && grifoActual != null && Input.GetKeyDown(KeyCode.E))
        {
            grifoActual.Cerrar();

            // Verificar si todos los grifos están cerrados
            if (TodosCerrados())
            {
                tareaCompletada = true;
                Debug.Log("✅ ¡Todos los grifos cerrados! Tarea completada.");
            }
        }
    }

    // Verifica si todos los grifos están cerrados
    bool TodosCerrados()
    {
        foreach (FaucetBehavior faucet in grifos)
        {
            if (!faucet.EstaCerrado) return false;
        }
        return true;
    }

    // Método para verificar si la tarea está completada
    public bool TareaCompletada()
    {
        return tareaCompletada;
    }

    // 🔄 NUEVO MÉTODO: Resetear la tarea de grifos
    public void ResetTask()
    {
        Debug.Log("🔄 Reseteando tarea de grifos...");

        tareaCompletada = false;
        cerca = false;
        grifoActual = null;

        // Resetear cada grifo individual
        foreach (FaucetBehavior faucet in grifos)
        {
            if (faucet != null)
            {
                faucet.ResetFaucet();
            }
        }

        Debug.Log("✅ Tarea de grifos reseteada");
    }

    // Mostrar mensaje de interacción en pantalla
    void OnGUI()
    {
        if (cerca && grifoActual != null && !grifoActual.EstaCerrado)
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