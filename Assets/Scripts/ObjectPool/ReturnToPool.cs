using System.Collections;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 2.0f;


    private void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
    }


    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        
        gameObject.SetActive(false); //PoolManager에서 큐에 넣는작업까지 했기 때문에, 비활성화만 해주면 된다.
    }
}
