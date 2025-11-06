using UnityEngine;

public class ToallaPickup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform toallaAnchor;
    public GameObject toallaVisualPrefab;
    public Transform puntoColocacion;

    private bool recogida = false;
    private bool entregada = false;
    private bool jugadorCerca = false;
    private bool cercaEntrega = false;

    void Update()
    {
        // Recoger la toalla
        if (jugadorCerca && !recogida && Input.GetKeyDown(KeyCode.E))
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
            cercaEntrega = false;

            Collider[] hits = Physics.OverlapSphere(playerMovement.transform.position, 2f);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("EntregaToalla"))
                {
                    cercaEntrega = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        entregada = true;
                        playerMovement.LlevarObjeto(false);
                        gameObject.SetActive(false);

                        if (toallaVisualPrefab != null && puntoColocacion != null)
                        {
                            // Ajuste opcional de altura si el pivot del prefab está centrado
                            Vector3 offset = new Vector3(0, 0.5f, 0);
                            Instantiate(toallaVisualPrefab, puntoColocacion.position + offset, puntoColocacion.rotation);
                        }

                        Debug.Log("Toalla entregada y colocada en el baño");
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }

    public bool ToallaEntregada => entregada;
}



