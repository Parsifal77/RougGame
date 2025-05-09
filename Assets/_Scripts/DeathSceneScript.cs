using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneScript : MonoBehaviour
{
    public void GoBackMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("RoomContent");
    }
}
