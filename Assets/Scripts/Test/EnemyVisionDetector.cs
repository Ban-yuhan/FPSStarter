using UnityEngine;

public class EnemyVisionDetector : MonoBehaviour
{
    public Transform targetPlayer;  // 감지할 대상(플레이어)

    public float detectionRadius = 10.0f;    // 감지 거리.

    public float viewAngle = 90.0f; // 시야각(전방 기준 좌우 벌어지는 각도)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DetectTarget();
    }

    private void Update()
    {
        DetectTarget();
    }

    /// <summary>
    /// 플레이어를 감지하는 함수.
    /// </summary>
    void DetectTarget()
    {
        // 적 -> 플레이어로 향하는 벡터 계산.
        // 목적지 - 출발지.
        // 벡터의 뺄셈을 이용해서 적에게서 플레이어로 가는 벡터를 만든다. 이 벡터는 방향과 거리 정보를 모두 담고 있다.
        Vector3 directionToTarget = targetPlayer.position - transform.position;

        // 거리 계산 (벡터의 길이)
        // magnitude는 벡터의 길이를 반환한다. 피타고라스의 정리를 사용
        float distanceToTarget = directionToTarget.magnitude;

        if(distanceToTarget > detectionRadius)
        {
            return;
        }

        // 방향 벡터 정규화.
        // 내적을 정확히 계산하기 위해 길이를 1로 만든다.
        Vector3 normalizeDirection = directionToTarget.normalized;

        // 내적 계산.
        // 적의 정면과 타겟 방향 간의 관계 계산.
        float dotResult = Vector3.Dot(transform.forward, normalizeDirection);

        // 내적 결과를 각도로 변환
        float angleToTarget = Mathf.Acos(dotResult) * Mathf.Rad2Deg;

        // 시야각 판정.
        // 전체 시야각의 절반보다 작은 각도에 있어야 시야 내에 있는 것이다.
        if(angleToTarget <= viewAngle * 0.5f)
        {
            Debug.Log("플레이어 발견! 죽여라!!!");
            Debug.DrawLine(transform.position, targetPlayer.position, Color.red);
            transform.LookAt(targetPlayer);
        }
        else
        {
            Debug.Log("주변에 기척은 있지만 보이지 않음");
            Debug.DrawLine(transform.position, targetPlayer.position, Color.yellow);
        }
    }
}
