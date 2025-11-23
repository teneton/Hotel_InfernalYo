using UnityEngine;

public class TrashPickUp : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform trashAnchor;
    public GameObject trashVisualPrefab;
    public Transform puntoColocacion;
    public Camera camaraJugador;
    public float radioInteraccion = 0.5f;
    public float distanciaInteraccion = 3.5f;

    private bool recogido = false;
    private bool entregado = false;
    private bool cerca = false;
    private bool cercaContenedor = false;

    // Sistema de conteo estático para todas las latas
    public static int latasRecogidas = 0;
    public static int totalLatas = 0;
    public static int latasEntregadas = 0;

    // Referencia al GameTaskManager
    private static GameTaskManager gameTaskManager;

    void Start()
    {
        // Contar esta lata en el total
        totalLatas++;
        Debug.Log($"Lata añadida. Total en escena: {totalLatas}");

        // Buscar el GameTaskManager si no está asignado
        if (gameTaskManager == null)
        {
            gameTaskManager = FindObjectOfType<GameTaskManager>();
        }
    }

    void Update()
    {
        cerca = DetectarBasura();

        if (!recogido && !playerMovement.EstaLlevandoObjeto && cerca && Input.GetKeyDown(KeyCode.E))
        {
            RecogerLata();
        }

        if (recogido && !entregado)
        {
            cercaContenedor = DetectarContenedor();
            if (cercaContenedor && Input.GetKeyDown(KeyCode.E))
            {
                EntregarLata();
            }
        }
    }

    void RecogerLata()
    {
        recogido = true;
        latasRecogidas++;
        playerMovement.LlevarObjeto(true, true);
        transform.SetParent(trashAnchor);
        transform.localPosition = new Vector3(0, -0.4f, 0);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 0.5f;
        GetComponent<Collider>().enabled = false;
        Debug.Log($"Lata recogida. Recogidas: {latasRecogidas}/{totalLatas}");
    }

    void EntregarLata()
    {
        entregado = true;
        latasEntregadas++;
        playerMovement.SoltarObjeto();
        gameObject.SetActive(false);

        if (trashVisualPrefab != null && puntoColocacion != null)
        {
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Instantiate(trashVisualPrefab, puntoColocacion.position + offset, puntoColocacion.rotation);
        }
        Debug.Log($"Lata entregada. Entregadas: {latasEntregadas}/{totalLatas}");

        // Verificar si todas las latas han sido entregadas
        if (TodasLasLatasEntregadas())
        {
            Debug.Log("¡TODAS LAS LATAS ENTREGADAS EN EL CONTENEDOR!");
            VerificarTareasCompletadas();
        }
    }

    // Método estático para verificar si todas las latas fueron ENTREGADAS
    public static bool TodasLasLatasEntregadas()
    {
        return latasEntregadas >= totalLatas;
    }

    // Método para verificar si ambas tareas están completas
    void VerificarTareasCompletadas()
    {
        if (gameTaskManager != null)
        {
            // Forzar la verificación de tareas en el GameTaskManager
            Debug.Log("Forzando verificacion de tareas completadas...");

            bool camaCompletada = false;
            BedTaskManager bedManager = FindObjectOfType<BedTaskManager>();
            if (bedManager != null)
            {
                camaCompletada = bedManager.TareaCompletada;
                Debug.Log($"Cama completada: {camaCompletada}");
            }

            if (camaCompletada && TodasLasLatasEntregadas())
            {
                Debug.Log("¡TODAS LAS TAREAS COMPLETADAS! Demonio debería calmarse.");
                // El GameTaskManager detectará esto automáticamente en su Update
            }
        }
        else
        {
            Debug.LogWarning("GameTaskManager no encontrado");
        }
    }

    // Propiedades estáticas para acceder desde otros scripts
    public static int LatasRecogidas => latasRecogidas;
    public static int LatasEntregadas => latasEntregadas;
    public static int TotalLatas => totalLatas;

    // Resetear contador (útil si reinicias el juego)
    public static void ResetearContador()
    {
        latasRecogidas = 0;
        latasEntregadas = 0;
        totalLatas = 0;
    }

    bool DetectarBasura()
    {
        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, radioInteraccion, out hit, distanciaInteraccion))
            return hit.collider != null && hit.collider.gameObject == gameObject;
        return false;
    }

    bool DetectarContenedor()
    {
        Collider[] hits = Physics.OverlapSphere(playerMovement.transform.position, 2f);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Contenedor"))
                return true;
        }
        return false;
    }

    public bool BasuraEntregada => entregado;

    void OnGUI()
    {
        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 40;
        estilo.normal.textColor = Color.white;
        estilo.alignment = TextAnchor.MiddleCenter;
        Rect mensaje = new Rect(Screen.width / 2 - 200, Screen.height - 120, 400, 80);

        if (cerca && !recogido)
            GUI.Label(mensaje, "Pulsa E para recoger basura", estilo);

        if (recogido && !entregado && cercaContenedor)
            GUI.Label(mensaje, "Pulsa E para entregar basura", estilo);
    }
}