using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider sensibilidadSlider;
    public Slider volumenSlider;

    public MouseLook mouseLookScript;

    void Start()
    {
        sensibilidadSlider.value = SettingsManager.Instance.sensibilidad;
        volumenSlider.value = SettingsManager.Instance.volumen;

        mouseLookScript.mouseSensitivity = SettingsManager.Instance.sensibilidad;
        AudioListener.volume = SettingsManager.Instance.volumen;

        sensibilidadSlider.onValueChanged.AddListener(SetSensibilidad);
        volumenSlider.onValueChanged.AddListener(SetVolumen);
    }



    public void SetSensibilidad(float value)
    {
        mouseLookScript.mouseSensitivity = value;
        SettingsManager.Instance.SetSensibilidad(value);
    }

    public void SetVolumen(float value)
    {
        AudioListener.volume = value;
        SettingsManager.Instance.SetVolumen(value);
    }



}
