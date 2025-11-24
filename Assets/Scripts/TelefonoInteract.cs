using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TelefonoInteract : MonoBehaviour
{
    public Camera camaraJugador;
    public float radioInteraccion = 0.5f;
    public float distanciaInteraccion = 3.5f;

    public GameObject canvasTelefono;
    public TMP_InputField inputCodigo;
    public Button botonConfirmar;

    public DemonBehaviour2 demonio2;
    public PlayerMovement playerMovement;

    private bool abierto = false;
    private bool cerca = false;
    private bool tareaCompletada = false; // NUEVO: Para marcar como completada

    // NUEVA propiedad pública
    public bool TareaCompletada => tareaCompletada;

    void Start()
    {
        canvasTelefono.SetActive(false);
        botonConfirmar.onClick.AddListener(ValidarCodigo);
    }

    void Update()
    {
        cerca = DetectarTelefono();

        // SOLO se puede interactuar si no está completada
        if (!tareaCompletada && !abierto && !playerMovement.EstaLlevandoObjeto && cerca && Input.GetKeyDown(KeyCode.E))
        {
            abierto = true;
            canvasTelefono.SetActive(true);
            inputCodigo.text = "";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Canvas del teléfono abierto");
        }
    }

    bool DetectarTelefono()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, radioInteraccion, out hit, distanciaInteraccion))
            return hit.collider != null && hit.collider.gameObject == gameObject;
        return false;
    }

    public void ValidarCodigo()
    {
        string codigoIngresado = inputCodigo.text;

        // MARCAR COMO COMPLETADA INDEPENDIENTEMENTE DEL RESULTADO
        tareaCompletada = true;

        if (codigoIngresado == "HAB-02")
        {
            Debug.Log("Código correcto, todo sigue igual");
        }
        else
        {
            Debug.Log("Código incorrecto, demonio enfadado!");
            if (demonio2 != null)
                demonio2.ActivarPersecucionRapida();
        }

        CerrarCanvas();
    }

    public void CerrarCanvas()
    {
        canvasTelefono.SetActive(false);
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