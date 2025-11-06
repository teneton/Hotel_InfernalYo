using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    public CleanerManager cleanerManager;
    public ToallaPickup toallaPickup;
    public DemonBehaviour demonBehaviour;

    public float tiempoTotal = 300f; // 5 minutos
    private float tiempoRestante;
    private bool persecucionActivada = false;
    private bool modoMatarActivado = false;

    void Start()
    {
        tiempoRestante = tiempoTotal;
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;

        bool limpiezaOk = cleanerManager != null && cleanerManager.LimpiezaCompletada;
        bool toallaOk = toallaPickup != null && toallaPickup.ToallaEntregada;
        bool tareasCompletas = limpiezaOk && toallaOk;

        if (tareasCompletas)
        {
            if (persecucionActivada || modoMatarActivado)
            {
                demonBehaviour.Calmar();
                Debug.Log("Tareas completadas: demonio calmado.");
            }
        }

        if (tiempoRestante <= 60f && !persecucionActivada && !tareasCompletas)
        {
            demonBehaviour.ActivarPersecucionSuave();
            persecucionActivada = true;
        }

        if (tiempoRestante <= 0f && !modoMatarActivado && !tareasCompletas)
        {
            demonBehaviour.ActivarModoMatar();
            modoMatarActivado = true;
        }
    }

    void OnGUI()
    {
        if (Time.timeScale == 0f) return;

        GUIStyle estiloTexto = new GUIStyle(GUI.skin.label);
        estiloTexto.fontSize = 20;
        estiloTexto.normal.textColor = Color.white;
        estiloTexto.alignment = TextAnchor.UpperCenter;

        float anchoBarra = 300f;
        float altoBarra = 25f;
        float x = Screen.width / 2 - anchoBarra / 2;
        float y = 60f;

        GUI.color = Color.gray;
        GUI.DrawTexture(new Rect(x, y, anchoBarra, altoBarra), Texture2D.whiteTexture);

        float porcentaje = tiempoRestante / tiempoTotal;
        GUI.color = Color.cyan;
        GUI.DrawTexture(new Rect(x, y, anchoBarra * porcentaje, altoBarra), Texture2D.whiteTexture);

        string texto = $"Tiempo global restante: {tiempoRestante:F1}s";
        GUI.color = Color.white;
        GUI.Label(new Rect(x, y - 30, anchoBarra, 30), texto, estiloTexto);
    }
}

