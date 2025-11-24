using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VentiladorInteract : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject canvasVentilador;
    public Slider sliderTiempo;
    public TMP_Text textoTiempo;
    public TMP_Dropdown dropdownPotencia;
    public Button botonConfirmar;

    [Header("Referencias externas")]
    public DemonBehaviour2 demonio2;
    public PlayerMovement playerMovement;

    [Header("Valores correctos")]
    public int potenciaCorrecta = 3;
    public int tiempoCorrecto = 90;

    private bool abierto = false;
    private bool cerca = false;
    private bool tareaCompletada = false; // NUEVO: Para marcar como completada

    // NUEVA propiedad pública
    public bool TareaCompletada => tareaCompletada;

    void Start()
    {
        canvasVentilador.SetActive(false);
        sliderTiempo.onValueChanged.AddListener(ActualizarTiempo);
        dropdownPotencia.onValueChanged.AddListener(ActualizarPotencia);
        botonConfirmar.onClick.AddListener(ValidarVentilador);
        ActualizarTiempo(sliderTiempo.value);
        ActualizarPotencia(dropdownPotencia.value);
    }

    void Update()
    {
        cerca = DetectarVentilador();

        // SOLO se puede interactuar si no está completada
        if (!tareaCompletada && !abierto && !playerMovement.EstaLlevandoObjeto && cerca && Input.GetKeyDown(KeyCode.E))
        {
            abierto = true;
            canvasVentilador.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Canvas del ventilador abierto");
        }
    }

    bool DetectarVentilador()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.5f, out hit, 3.5f))
            return hit.collider != null && hit.collider.gameObject == gameObject;
        return false;
    }

    void ActualizarTiempo(float valor)
    {
        textoTiempo.text = valor.ToString("F0") + " min";
    }

    void ActualizarPotencia(int indice)
    {
        Debug.Log("Potencia seleccionada: " + dropdownPotencia.options[indice].text);
    }

    public void ValidarVentilador()
    {
        int tiempo = (int)sliderTiempo.value;
        int potencia = int.Parse(dropdownPotencia.options[dropdownPotencia.value].text);

        // MARCAR COMO COMPLETADA INDEPENDIENTEMENTE DEL RESULTADO
        tareaCompletada = true;

        if (tiempo == tiempoCorrecto && potencia == potenciaCorrecta)
        {
            Debug.Log("Ventilador configurado correctamente");
        }
        else
        {
            Debug.Log("Ventilador configurado incorrectamente, demonio enfadado!");
            if (demonio2 != null)
                demonio2.ActivarPersecucionRapida();
        }

        CerrarCanvas();
    }

    void CerrarCanvas()
    {
        canvasVentilador.SetActive(false);
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
