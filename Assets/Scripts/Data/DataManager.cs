using UnityEngine;
using System.IO; //파일 입출력 기능을 사용하기 위한 네임스페이스

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameData currentData;

    private string savePath; //저장 경로를 담을 string형 변수


    private void Awake()
    {
        Instance = this;

        // 저장 경로 설정 : 유니티가 제공하는 내부 저장소의 경로 + 저장할 파일의 이름을 합쳐서 최종 경로 문자열을 생성.
        savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        //Path.Combine : 두 개의 문자열을 합쳐줌

        LoadGame();
        LoadGameByPlayerPrefs();
    }


    public void SaveGame()
    {
        //클래스의 멤버 변수들을 JSON 문자열로 변환(직렬화)
        string jsonString = JsonUtility.ToJson(currentData, true);
        //두 번째인자가 true인 경우 사람이 알아보기 쉽게 줄바꿈까지 처리해줌. 두 번째 인자가 없는경우 기본 flase

        //File : system.IO 네임스페이스에서 제공하는 클래스
        File.WriteAllText(savePath, jsonString);
        //writeAllText : 해당 경로(svaePath)에 Json문자열(jsonString)을 파일로 작성

        Debug.Log("내부 저장소 경로 : " + savePath);
        Debug.Log("저장된 내용 : " + jsonString);
    }


    public void SaveGameByPlayerPrefs()
    {
        // int, float, string형의 자료만 저장 가능.
        PlayerPrefs.SetInt("Score", 100);
        PlayerPrefs.SetFloat("HP", 50.0f);
        PlayerPrefs.SetString("Name", "Hero"); //string형 변수에 이름이 저장되어있다면 변수를 넣어줌.

        Vector3 position = new Vector3(1.0f, 10.0f, 20.0f);
        PlayerPrefs.SetFloat("PosX", position.x);
        PlayerPrefs.SetFloat("PosY", position.y);
        PlayerPrefs.SetFloat("PosZ", position.z);

        Debug.Log("Prefs를 통한 저장 완료");
    }


    public void LoadGameByPlayerPrefs()
    {
        int score = PlayerPrefs.GetInt("Score", 500); //저장이 되어있는경우 식별자("")의 데이터를, 데이터가 없는 경우 뒤의 값을 넣어준다.
        float hp = PlayerPrefs.GetFloat("HP", 10.0f);
        string name = PlayerPrefs.GetString("Name", "None");

        float posX = PlayerPrefs.GetFloat("PosX", 0f);
        float posY = PlayerPrefs.GetFloat("PosY", 0f);
        float posZ = PlayerPrefs.GetFloat("PosZ", 0f);
    }


    public void LoadGame()
    {
        if(File.Exists(savePath) == true) //해당 경로의 파일이 존재하는지 여부 체크
        {
            string jsonString = File.ReadAllText(savePath); //파일에서 문자열을 읽음

            currentData = JsonUtility.FromJson<GameData>(jsonString); //FromJson<변수의 자료형>(읽어온 데이터)

            Debug.Log("게임 로드 완료");
        }
        else
        {
            currentData = new GameData();
            Debug.Log("저장된 파일이 존재하지 않습니다.");
        }
    }


    //게임이 종료될 떄 유니티가 자동으로 호출하는 함수
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
