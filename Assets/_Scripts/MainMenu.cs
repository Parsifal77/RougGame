using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    // Delay in seconds before the scene is loaded.
    public float delayBeforeLoad = 1f;

    // This method can be linked to your button's OnClick event.
    public void PlayGame()
    {
        // Start the coroutine that waits before loading the scene.
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(delayBeforeLoad);
        // Then load the scene named "RoomContent".
        SceneManager.LoadScene("RoomContent");
    }
}
