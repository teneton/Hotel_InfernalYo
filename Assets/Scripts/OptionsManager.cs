using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider sensibilidadSlider;
    public Slider volumenSlider;

    public MouseLook mouseLookScript;

    void Start()
    {
        // Inicializa sliders con valores actuales
        sensibilidadSlider.value = mouseLookScript.mouseSensitivity;
        volumenSlider.value = AudioListener.volume;

        // Asigna listeners para que se actualicen al mover el slider
        sensibilidadSlider.onValueChanged.AddListener(SetSensibilidad);
        volumenSlider.onValueChanged.AddListener(SetVolumen);
    }

    public void SetSensibilidad(float value)
    {
        mouseLookScript.mouseSensitivity = value;
    }

    public void SetVolumen(float value)
    {
        AudioListener.volume = value;
    }
}
