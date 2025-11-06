using UnityEngine;

public class ToallaPickup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform toallaAnchor;
    public GameObject toallaVisualPrefab;
    public Transform puntoColocacion;

    public Camera camaraJugador;
    public float radioInteraccion = 0.5f;
    public float distanciaInteraccion = 3.5f;

    private bool recogida = false;
    private bool entregada = false;

    void Update()
    {
        // Detectar si el jugador está mirando a la toalla
        if (!recogida && DetectarToalla() && Input.GetKeyDown(KeyCode.E))
        {
            recogida = true;
            playerMovement.LlevarObjeto(true);

            transform.SetParent(toallaAnchor);
            transform.localPosition = new Vector3(0, -0.4f, 0);
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one * 0.5f;

            GetComponent<Collider>().enabled = false;
            Debug.Log("Toalla recogida");
        }

        // Detectar si estamos cerca de la zona de entrega
        if (recogida && !entregada)
        {
            Collider[] hits = Physics.OverlapSphere(playerMovement.transform.position, 2f);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("EntregaToalla"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        entregada = true;
                        playerMovement.LlevarObjeto(false);
                        gameObject.SetActive(false);

                        if (toallaVisualPrefab != null && puntoColocacion != null)
                        {
                            Vector3 offset = new Vector3(0, 0.5f, 0);
                            Instantiate(toallaVisualPrefab, puntoColocacion.position + offset, puntoColocacion.rotation);
                        }

                        Debug.Log("Toalla entregada y colocada en el baño");
                    }
                }
            }
        }
    }

    bool DetectarToalla()
    {
        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, radioInteraccion, out hit, distanciaInteraccion))
        {
            return hit.collider != null && hit.collider.gameObject == gameObject;
        }

        return false;
    }

    public bool ToallaEntregada => entregada;
}




