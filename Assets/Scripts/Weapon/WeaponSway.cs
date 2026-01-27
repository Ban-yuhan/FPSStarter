using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float swayMultiplier = 0.02f; //흔들림 강도
    
    [SerializeField]
    private float maxAmount = 0.06f; //최대 이동 제한

    [SerializeField]
    private float smooth = 6.0f; //부드러움 정도.

    
    private Vector3 initialPosition; //초기 위치 저장용 변수

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        //마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        //이동 범위 제한
        mouseX = Mathf.Clamp(mouseX, -maxAmount, maxAmount);
        mouseY = Mathf.Clamp(mouseY, -maxAmount, maxAmount);

        //목표 위치 계산
        Vector3 finalPosition = new Vector3(-mouseX, -mouseY, 0.0f) + initialPosition;

        //부드로운 이동 처리
        //현재위치에서 목표 위치로 smooth의 속도로 서서히 이동
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, smooth*Time.deltaTime);  
    }

}
