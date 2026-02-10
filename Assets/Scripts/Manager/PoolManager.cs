using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// 직렬화 클래스. 다른 클래스에서 이 클래스 타입의 변수를 선언하면 인스펙터 창에서 보이게 된다.
/// 멤버 변수들이 인스펙터에 노출되게 하기 위해 직렬화로 만듦.
/// </summary>
[System.Serializable] 
public class Pool
{
    public string tag; //풀의 이름
    public GameObject prefab; //생서할 프리팹
    public int size; //미리 생성할 개수.
}


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField]
    private List<Pool> pools; //인스펙터에서 설정할 풀 목록.

    [SerializeField]
    private Dictionary<string, Queue<GameObject>> poolDictionary; //실제 오브젝트들을 담을 창고(이름, 오브젝트들의 큐).

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //for(int i = 0; i<pools.Count; ++i)
        foreach(Pool pool in pools) //pools리스트의 처음부터 끝까지 순회하며 pool변수에 들어감
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; ++i)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);  //생성된 오브젝트들을 풀매니저의 자식으로 설정. (계층창에서 풀매니저 아래에 생성된 오브젝트들이 보이게 됨)

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag) == false) //Dictionary에 해당 태그가 없으면
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); //큐에서 오브젝트 하나 꺼냄

        //오브젝트 활성화 및 위치, 회전 설정
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;


        //사용하려는 객체에 초기화 코드가 있다면 여기서 실행.(이전에 사용했던 기록을 초기화)


        poolDictionary[tag].Enqueue(objectToSpawn); //사용한 오브젝트를 다시 큐 맨 뒤로 보냄.

        return objectToSpawn;

    }
}
