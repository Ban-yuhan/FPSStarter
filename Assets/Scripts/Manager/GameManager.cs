using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DataManager.Instance.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DataManager.Instance.SaveGameByPlayerPrefs();
        }
    }
}
