using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            bool doHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3.0f);

            if (doHit)
            {
                IInteractable interactObj = hit.transform.GetComponent<IInteractable>();
                if (interactObj != null)
                {
                    interactObj.Interact();
                }
            }
        }    
    }
}
