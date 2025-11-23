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

    public DemonBehaviour2 demonio2;
    public PlayerMovement playerMovement;

    void Start()
    {
        canvasTermometro.SetActive(false);
        sliderTemperatura.onValueChanged.AddListener(ActualizarTexto);
        botonConfirmar.onClick.AddListener(ValidarTemperatura);
    }

    void Update()
    {
        cerca = DetectarTermometro();

        if (!abierto && !playerMovement.EstaLlevandoObjeto && cerca && Input.GetKeyDown(KeyCode.E))
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
        if (valor >= minCorrecto && valor <= maxCorrecto)
        {
            Debug.Log("Temperatura correcta: " + valor);
            CerrarCanvas();
        }
        else
        {
            Debug.Log("Temperatura incorrecta: " + valor);
            CerrarCanvas();
            if (demonio2 != null)
                demonio2.ActivarPersecucionRapida();
        }
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
        if (cerca && !abierto)
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
