using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6.0f;

    [SerializeField]
    private int runMultiplier = 2;

    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private int MaxJumpCount = 2;

    [SerializeField]
    private float gravity = -9.8f; //중력은 아래쪽으로 작용 -> -값

    [SerializeField]
    private float groundCheckDistance = 0.4f; //땅에 닿아있는지 체크하기 위한 여유 거리

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Transform groundCheck; //발 및 체크를위한 transform

    [SerializeField]
    private CharacterController controller;

    private Vector3 velocity; //낙하 속도 지정을 위한 Vector3 변수

    private bool isGrounded;

    private int JumpCount;


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask); //bool형을 반환
        
        if(isGrounded && velocity.y < 0) //지면에 있고, y속도가 0보다 작으면 -> 아래로 떨어지려고 할 때
        {
            //0으로 맞춰 줄 경우 땅에서 살짝 뜨는 판정이 뜰 수 있음. -> 작은 음수값을 넣어줌으로서 살짝 눌러주는 역할
            velocity.y = -2.0f; 
            JumpCount = MaxJumpCount;
        }

        PlayerMove();
        
        jump();
        
        velocity.y += gravity * Time.deltaTime; //점프후 천천히 내려오게 하기 위해 y축 속도에 매 프레임마다 중력 적용

        // 낙하 이동 처리
        controller.Move(velocity * Time.deltaTime);
    }


    void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical"); //3D는 2D와 다르게 y축이아닌 z축으로 이동


        //이동방향 벡터 계산
        Vector3 move = transform.right * x + transform.forward * z; //내가 어느쪽으로 이동할 것인지 방향벡터 계산

        //GetKeyDown과 GetKey의 차이
        //GetKeyDown : 누른 그 프레임 한 번만 적용(누르고 있어도 누른 그 프레임만 적용)
        //GetKey : 누르고 있는 동안 적용
        float speed = Input.GetKey(KeyCode.LeftControl)? moveSpeed*runMultiplier : moveSpeed;
                
        controller.Move(move * speed * Time.deltaTime);
    }


    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCount > 0)
        {
            //점프 공식 : sqrt(높이 * -2.0f * 중력)
            // 높이만큼 뛰기 위해 필요한 순간 속력을 구하는 물리 공식 -> from 등가속도 운동 공식
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity); //sqrt : 제곱근
            JumpCount -= 1;
        }
    }
}
