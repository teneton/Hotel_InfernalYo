using UnityEngine;

public class ControlConductos : MonoBehaviour
{
    public GameObject[] murosConducto; // arrastra las 6 paredes aquí

    void Start()
    {
        bool estanLimpios = GameManager.instancia.conductosLimpios;

        foreach (GameObject muro in murosConducto)
        {
            muro.SetActive(!estanLimpios); // si NO se limpian, aparecen
        }
    }
}

