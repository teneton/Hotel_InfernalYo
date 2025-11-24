using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TermometroInteract : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject canvasTermometro;
    public Slider sliderTemperatura;
    public TMP_Text textoTemperatura;
    public Button botonConfirmar;

    [Header("Rango correcto")]
    public float minCorrecto = 23f;
    public float maxCorrecto = 25f;

    private bool abierto = false;
    private bool cerca = false;
    private bool tareaCompletada = false; // NUEVO: Para marcar como completada

    public DemonBehaviour2 demonio2;
    public PlayerMovement playerMovement;

    // NUEVA propiedad pública
    public bool TareaCompletada => tareaCompletada;

    void Start()
    {
        canvasTermometro.SetActive(false);
        sliderTemperatura.onValueChanged.AddListener(ActualizarTexto);
        botonConfirmar.onClick.AddListener(ValidarTemperatura);
    }

    void Update()
    {
        cerca = DetectarTermometro();

        // SOLO se puede interactuar si no está completada
        if (!tareaCompletada && !abierto && !playerMovement.EstaLlevandoObjeto && cerca && Input.GetKeyDown(KeyCode.E))
        {
            abierto = true;
            canvasTermometro.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Canvas del termómetro abierto");
        }
    }

    bool DetectarTermometro()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.5f, out hit, 3.5f))
            return hit.collider != null && hit.collider.gameObject == gameObject;
        return false;
    }

    void ActualizarTexto(float valor)
    {
        textoTemperatura.text = valor.ToString("F1") + " °C";
    }

    public void ValidarTemperatura()
    {
        float valor = sliderTemperatura.value;

        // MARCAR COMO COMPLETADA INDEPENDIENTEMENTE DEL RESULTADO
        tareaCompletada = true;

        if (valor >= minCorrecto && valor <= maxCorrecto)
        {
            Debug.Log("Temperatura correcta: " + valor);
        }
        else
        {
            Debug.Log("Temperatura incorrecta: " + valor);
            if (demonio2 != null)
                demonio2.ActivarPersecucionRapida();
        }

        CerrarCanvas();
    }

    void CerrarCanvas()
    {
        canvasTermometro.SetActive(false);
        abierto = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        // SOLO mostrar mensaje si no está completada
        if (cerca && !abierto && !tareaCompletada)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.label);
            estilo.fontSize = 40;
            estilo.normal.textColor = Color.white;
            estilo.alignment = TextAnchor.MiddleCenter;
            Rect mensaje = new Rect(Screen.width / 2 - 200, Screen.height - 120, 400, 80);
            GUI.Label(mensaje, "Pulsa E para interactuar", estilo);
        }
    }
}
