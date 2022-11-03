using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneManagment : MonoBehaviour
{
    public void SceneToLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        Scene scn = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scn.name);
    }




    // --------------------------- InvokeOnSceneIndex ---------------------------
    public UnityEvent invokeMethod;//set in editor

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        invokeMethod.Invoke();
    }


    private int CheckSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void EnableOnSceneIndex(GameObject section, int sceneIndex)
    {
        if (section == null)
            return;

        if (sceneIndex == CheckSceneIndex())
        {
            section.SetActive(true);
        }
    }

    public void DisableOnSceneIndex(GameObject[] sections, int sceneIndex)
    {
        if (sections == null)
            return;

        if (sceneIndex == CheckSceneIndex())
        {
            foreach (GameObject section in sections)
            {
                section.SetActive(false);
            }
        }
    }
    // --------------------------- InvokeOnSceneIndex ---------------------------
}
