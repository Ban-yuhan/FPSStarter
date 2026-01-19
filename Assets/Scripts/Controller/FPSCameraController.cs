using TMPro;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 200.0f; //마우스 민감도

    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0.0f;

    private bool isReverse;

    [SerializeField]
    private float SightRadius = 90f;

    [SerializeField]
    private TMP_Text SightTMP;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서가 화면 중앙에 고정.
        Cursor.visible = false; //커서가 보이지 않도록 함
        SightTMP.text = SightRadius.ToString();

    }


    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //상하 회전 계산
        //마우스를 위로 올리면 시선이 위를 보도록
        //Unity 회전규칙(오른손 좌표계 + 내부정의) -> Pitch의 경우 양수(+)는 아래로, 음수(-)는 위로 회전.
        //따라서 GetAxis에서 마우스를 올릴 때 증가한 값을 빼 주어야 고개를 위로 들도록 할 수 있음
        if(!isReverse)
            xRotation -= mouseY;
        
        else if(isReverse)
            xRotation += mouseY;

        xRotation = Mathf.Clamp(xRotation, -SightRadius, SightRadius); //범위. XRotation값이 -90~90을 벗어날 수 없음

        //오브젝트의 회전값에 접근, X축 기준 회전값을 조정
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        //Y축 기준 회전(Yaw)는 playerBody에 오브젝트를 연결해 오브젝트 자체를 회전)
        if(playerBody != null)
        {
            //Vector3.up == (0, 1, 0)을 곱해 Y축 성분의 회전벡터만을 남김으로서 Pitch와 Roll이 변경되지 않도록 함 -> 어느 축 기준으로 회전할 지
            // Pitch의 경우 Vector3.right == (1, 0, 0)를, Roll의 경우 Vector3.forward == (0, 0, 1)를 곱해줌
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void ChangeMouseReverse(bool isOn)
    {
        isReverse = isOn;
        Debug.Log("isReverse : " + isReverse);
    }
    
    public void ChangeSightRadius(float radiusRate)
    {
        SightRadius = 90f * radiusRate;
        SightTMP.text = SightRadius.ToString();
    }
}
