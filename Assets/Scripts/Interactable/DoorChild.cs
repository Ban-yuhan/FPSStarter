using UnityEngine;

public class DoorChild : MonoBehaviour, IInteractable
{
    private Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    public void Interact()
    {
        if (door != null)
        {
            door.Interact();
        }
    }
}
