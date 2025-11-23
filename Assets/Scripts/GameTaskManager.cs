using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTaskManager : MonoBehaviour
{
    public BedTaskManager bedTaskManager;
    public DemonBehaviour demonBehaviour;
    public GameObject juegoFinalizadoCanvas;

    public float tiempoTotal = 210f;
    private float tiempoRestante;
    private bool persecucionActivada = false;
    private bool modoMatarActivado = false;
    private bool tareasCompletadas = false;

    void Start()
    {
        tiempoRestante = tiempoTotal;

        if (juegoFinalizadoCanvas != null)
            juegoFinalizadoCanvas.SetActive(false);

        Debug.Log($"GameTaskManager iniciado - Latas totales: {TrashPickUp.TotalLatas}");
    }

    void Update()
    {
        if (!tareasCompletadas)
        {
            tiempoRestante -= Time.deltaTime;
        }

        // Verificar estado de las tareas
        bool latasOk = TrashPickUp.TodasLasLatasEntregadas();
        bool camaOk = bedTaskManager != null && bedTaskManager.TareaCompletada;
        bool nuevasTareasCompletas = latasOk && camaOk;

        // DEBUG cada segundo
        if (Time.frameCount % 60 == 0 && !tareasCompletadas)
        {
            Debug.Log($"DEBUG - Latas: {TrashPickUp.LatasEntregadas}/{TrashPickUp.TotalLatas} = {latasOk}, " +
                     $"Cama: {camaOk}, Completas: {nuevasTareasCompletas}, " +
                     $"Tiempo: {tiempoRestante:F1}s");
        }

        if (nuevasTareasCompletas && !tareasCompletadas)
        {
            tareasCompletadas = true;
            Debug.Log("¡TAREAS COMPLETADAS! Calmando demonio...");
            CalmarDemonio();
        }

        if (!tareasCompletadas)
        {
            if (tiempoRestante <= 60f && !persecucionActivada)
            {
                demonBehaviour.ActivarPersecucionSuave();
                persecucionActivada = true;
                Debug.Log("Demonio enfadado! Quedan menos de 60 segundos.");
            }

            if (tiempoRestante <= 0f && !modoMatarActivado)
            {
                demonBehaviour.ActivarModoMatar();
                modoMatarActivado = true;
                Debug.Log("MODO MATAR ACTIVADO! Demonio viene a por ti.");
            }
        }
    }

    void CalmarDemonio()
    {
        Debug.Log("Calmando demonio...");

        if (demonBehaviour != null)
        {
            demonBehaviour.Calmar();
            Debug.Log("Demonio calmado permanentemente.");
        }
    }

    void OnGUI()
    {
        if (Time.timeScale == 0f) return;

        GUIStyle estiloTexto = new GUIStyle(GUI.skin.label);
        estiloTexto.fontSize = 34;
        estiloTexto.normal.textColor = Color.white;
        estiloTexto.alignment = TextAnchor.UpperLeft;

        float anchoBarra = 520f;
        float altoBarra = 45f;
        float x = 35f;
        float y = 55f;

        GUI.color = Color.gray;
        GUI.DrawTexture(new Rect(x, y, anchoBarra, altoBarra), Texture2D.whiteTexture);

        float porcentaje = tiempoRestante / tiempoTotal;
        GUI.color = tareasCompletadas ? Color.green : Color.cyan;
        GUI.DrawTexture(new Rect(x, y, anchoBarra * porcentaje, altoBarra), Texture2D.whiteTexture);

        string texto = tareasCompletadas ? "TAREAS COMPLETADAS - DEMONIO CALMADO" : $"Tiempo: {tiempoRestante:F1}s";
        GUI.color = tareasCompletadas ? Color.green : Color.white;
        GUI.Label(new Rect(x, y - 45, anchoBarra, 45), texto, estiloTexto);

        string textoLatas = $"Latas: {TrashPickUp.LatasEntregadas}/{TrashPickUp.TotalLatas}";
        GUI.Label(new Rect(x, y + 60, anchoBarra, 45), textoLatas, estiloTexto);

        if (bedTaskManager != null)
        {
            string textoCama = $"Cama: {(bedTaskManager.TareaCompletada ? "COMPLETADA" : "PENDIENTE")}";
            GUI.Label(new Rect(x, y + 110, anchoBarra, 45), textoCama, estiloTexto);
        }

        string estadoDemonio = tareasCompletadas ? "DEMONIO: CALMADO" : (modoMatarActivado ? "DEMONIO: MODO MATAR" : (persecucionActivada ? "DEMONIO: ENFADADO" : "DEMONIO: TRANQUILO"));
        GUI.Label(new Rect(x, y + 160, anchoBarra, 45), estadoDemonio, estiloTexto);
    }
}