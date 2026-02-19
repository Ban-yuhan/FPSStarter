using UnityEngine;
using UnityEngine.Rendering; //volume 사용
using UnityEngine.Rendering.Universal; //URP효과 사용

public class DamageVignette : MonoBehaviour
{
    [SerializeField]
    private Volume globalVolume; //씬에 있는 Global Volume을 연동하기 위한 변수

    private Vignette vignette; //제어할 비네팅 효과를 담을 변수

    [SerializeField]
    private float maxIntensity = 0.5f; // 최대 강도(체력이 0일 때)

    [SerializeField]
    private float pulseSpeed = 5.0f; //펄스 효과 깜박임 속도


    private void Start()
    {
        bool result = globalVolume.profile.TryGet(out vignette);

        if(result == true)
        {
            Debug.Log("비네팅 효과 찾음!!");
        }
    }


    public void UpdateVignette(float healthPercent)
    {
        if(vignette == null)
        {
            return;
        }

        //체력이 낮을수록 강도(Intensity)가 높아지도록 계산
        float intensity = (1.0f - healthPercent) * maxIntensity;

        //비네팅 값 적용
        vignette.intensity.value = intensity;

        //체력이 30% 미만이면 빨간색으로 펄스 효과
        if(healthPercent < 0.3f)
        {
            vignette.color.value = Color.red;

            //깜박임
            vignette.intensity.value += Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
        }
        else 
        {
            vignette.color.value = Color.black;
        }
    }
}
