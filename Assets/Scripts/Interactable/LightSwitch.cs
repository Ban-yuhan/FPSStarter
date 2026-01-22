using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Light light;

    public void Interact()
    {
        if (light != null)
        {
            light.enabled = !light.enabled;
            Debug.Log("전등 스위치 조작");
        }
    }
}
