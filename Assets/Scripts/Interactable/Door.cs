using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen; //반전 : 현재 true면 false로, false면 true로 바꾸어줌

        float targetY = isOpen == true ? -90.0f : 0f;
        transform.rotation = Quaternion.Euler(0f, targetY, 0f);
        Debug.Log(isOpen == true ? "문이 열렸습니다" : "문이 닫혔습니다");
    }
}
