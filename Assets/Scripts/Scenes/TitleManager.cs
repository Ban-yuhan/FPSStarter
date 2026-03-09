using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickStartButton()
    {
        Debug.Log("게임을 시작합니다");

        SceneManager.LoadScene("LoadingScene");
    }


    public void OnClickExitButton()
    {
        Debug.Log("게임을 종료합니다.");

        //게임을 종료할 때 호출하는 함수. 에디터에서는 작동X
        Application.Quit();
    }
}
