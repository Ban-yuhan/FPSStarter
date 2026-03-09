using UnityEngine;

//소리를 들을 수 있는 모든 오브젝트에 추가
public interface INoiseHearable
{
    /// <summary>
    /// 소음의 위치와 강도를 전달받는 함수
    /// </summary>
    /// <param name="noisePosition"> 소음의 위치 </param>
    /// <param name="intensity"> 소음의 강도 </param>
    void OnHearNoise(Vector3 noisePosition, float intensity);
}
