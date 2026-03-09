using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    
    [SerializeField]
    private float interactRange = 3.0f; //손이 닿는 거리

    [SerializeField]
    private LayerMask interactableMask; //아이템 레이어만 검사

    [SerializeField]
    private Text interactPromptText; //화면 중앙에 표시할 안내 텍스트


    private void Start()
    {
        interactPromptText.text = string.Empty; //처음에는 안내 텍스트를 비워둠(텍스트 UI를 빈 문자열로 초기화)
    }


    void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //화면(카메라)의 정 중앙에서 레이를 발사
        RaycastHit hit;

        //레이케스트
        if(Physics.Raycast(ray, out hit, interactRange, interactableMask) == true) //광선에 맞은 아이템 오브젝트가 있다면
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null)
            {
                if(interactPromptText != null)
                {
                    interactPromptText.text = interactable.GetInteractText();
                }

                if(Input.GetKeyDown(KeyCode.F) == true)
                {
                    interactable.Interact(gameObject);
                }
                return;
            }
        }

        //광선에 맞은 아이템 오브젝트가 없다면
        if(interactPromptText != null)
        {
            //문자열 초기화
            interactPromptText.text = string.Empty;
        }
    }
}
