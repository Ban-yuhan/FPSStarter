using UnityEngine;
using System; //event Action 사용을 위해 추가하는 네임스페이스

/// <summary>
/// 정적 클래스
/// </summary>
public static class MissionEventBus
{
    //적이 죽었을 때 발생하는 이벤트
    public static event Action OnEnemyKilled;

    //플레이어가 특정 구역에 도달했을 때 발생하는 이벤트
    public static event Action<string> OnAreaReached;

     public static void PublishEnemyKilled()
     {
        if(OnEnemyKilled != null)
        {
            OnEnemyKilled.Invoke();
        }
     }


    public static void PublishAreaReached(string areaName)
    {
        if(OnAreaReached != null)
        {
            OnAreaReached.Invoke(areaName);
        }
    }
}
