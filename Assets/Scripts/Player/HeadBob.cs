using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private float walkBobbingSpeed = 13.0f; //흔들리는 속도

    [SerializeField]
    private float bobbingAmount = 0.05f; //흔들리는 정도(진폭)

    [SerializeField]
    private FPSMovement playerMovement;

    private float timer = 0.0f;
    private float defaultPosY = 0.0f;
    


    private void Start()
    {
        defaultPosY = transform.localPosition.y;
    }


    private void Update()
    {
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) //내가 좌우 또는 앞뒤로 이동중이라면
        {
            timer += Time.deltaTime * walkBobbingSpeed;

            // -1 ~ 1 사이를 오가는 값이 나온다.
            float waveSlice = Mathf.Sin(timer);

            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + waveSlice * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            timer = 0.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkBobbingSpeed), transform.localPosition.z);
        }
    }
}
