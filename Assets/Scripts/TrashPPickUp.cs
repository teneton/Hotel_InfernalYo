using UnityEngine;

public class TrashPickUp : MonoBehaviour
{
    public PlayerMovement playerMovement;      // Referencia al movimiento del jugador
    public Transform trashAnchor;              // Punto donde el jugador sujeta la basura
    public GameObject trashVisualPrefab;       // Modelo visual que aparece en el contenedor
    public Transform puntoColocacion;          // Lugar donde se coloca el objeto entregado

    public Camera camaraJugador;               // Cámara del jugador
    public float radioInteraccion = 0.5f;      // Radio del SphereCast
    public float distanciaInteraccion = 3.5f;  // Distancia máxima para interactuar

    private bool recogido = false;             // Si el jugador ya ha recogido esta basura
    private bool entregado = false;            // Si ya se ha entregado en el contenedor

    void Update()
    {
        // RECoger basura si:
        // - No está recogida
        // - El jugador NO lleva nada
        // - La mira
        // - Pulsa E
        if (!recogido && !playerMovement.EstaLlevandoObjeto && DetectarBasura() && Input.GetKeyDown(KeyCode.E))
        {
            recogido = true;

            // La basura reduce la velocidad igual que la toalla
            playerMovement.LlevarObjeto(true, true);

            // Colocar el objeto en la mano del jugador
            transform.SetParent(trashAnchor);
            transform.localPosition = new Vector3(0, -0.4f, 0);
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one * 0.5f;

            // Evitamos colisiones mientras la llevamos
            GetComponent<Collider>().enabled = false;

            Debug.Log("Basura recogida");
        }

        // ENTREGAR BASURA EN CONTENEDOR
        if (recogido && !entregado)
        {
            Collider[] hits = Physics.OverlapSphere(playerMovement.transform.position, 2f);

            foreach (Collider hit in hits)
            {
                // Solo contenedores válidos (tag Contenedor)
                if (hit.CompareTag("Contenedor") && Input.GetKeyDown(KeyCode.E))
                {
                    entregado = true;

                    // Soltamos el objeto desde el sistema del jugador
                    playerMovement.SoltarObjeto();

                    // Desactivamos este objeto
                    gameObject.SetActive(false);

                    // Instanciamos versión visual en el contenedor
                    if (trashVisualPrefab != null && puntoColocacion != null)
                    {
                        Vector3 offset = new Vector3(0, 0.5f, 0);
                        Instantiate(trashVisualPrefab, puntoColocacion.position + offset, puntoColocacion.rotation);
                    }

                    Debug.Log("Basura entregada en contenedor");
                }
            }
        }
    }

    // Detectar si estamos mirando la basura
    bool DetectarBasura()
    {
        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, radioInteraccion, out hit, distanciaInteraccion))
            return hit.collider != null && hit.collider.gameObject == gameObject;

        return false;
    }

    public bool BasuraEntregada => entregado;
}
