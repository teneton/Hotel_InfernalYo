using UnityEngine;

public class SettingsBootstrap : MonoBehaviour
{
    void Awake()
    {
        if (SettingsManager.Instance == null)
        {
            GameObject settingsPrefab = Resources.Load<GameObject>("Assets/Managers");
            if (settingsPrefab != null)
            {
                Instantiate(settingsPrefab);
                Debug.Log("SettingsManager instanciado en Hotel");
            }
            else
            {
                Debug.LogError("No se encontró el prefab SettingsManager ");
            }
        }
    }
}

