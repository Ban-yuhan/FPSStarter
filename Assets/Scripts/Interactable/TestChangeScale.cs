using UnityEngine;

public class TestChangeScale : MonoBehaviour, IInteractable
{
    private Vector3 OriginScale;
    private Vector3 BiggerScale;
    bool isInteract;
    private void Awake()
    {
        OriginScale = transform.localScale;
        BiggerScale = transform.localScale * 3; ;
    }

    public void Interact()
    {
        isInteract = !isInteract;
        transform.localScale = isInteract == true ? BiggerScale : OriginScale;
    }
}
