using UnityEngine;

public interface IInteractable 
{
    //상호작용을 실행하는 함수. (누가 나(오브젝트의 입장)를 클릭했는지 알기 위해 player 객체를 받음)
    void Interact(GameObject player);


    //화면에 띄울 텍스트 (예 : F를 눌러 구급상자 획득)
    string GetInteractText();
}
