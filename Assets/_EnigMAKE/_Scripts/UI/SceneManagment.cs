using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
struct scenelist
{
    public UnityEvent invokeMethod; //set in editor
    public int sceneId;
}





public class SceneManagment : MonoBehaviour
{

    // --------------------------- Loading Methods ---------------------------

    public void SceneToLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        Scene scn = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scn.name);
    }

    // --------------------------------------------------------------------------



    // --------------------------- InvokeOnSceneIndex ---------------------------


    [SerializeField]
    private scenelist[] scenelists;

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
       
        if (scenelists == null)
            return;


        int id = CheckSceneIndex();
        for (int i = 0; i < scenelists.Length; i++)
        {
            if (scenelists[i].sceneId == id)
            {
                scenelists[i].invokeMethod.Invoke();
                //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenelists[i].sceneId);
            }

            //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenelists[i].sceneId);
        }


    }


    private int CheckSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    //public void EnableOnSceneIndex(GameObject section, int sceneIndex)
    //{
    //    if (section == null)
    //        return;

    //    if (sceneIndex == CheckSceneIndex())
    //    {
    //        section.SetActive(true);
    //    }
    //}

    //public void DisableOnSceneIndex(GameObject[] sections, int sceneIndex)
    //{
    //    if (sections == null)
    //        return;

    //    if (sceneIndex == CheckSceneIndex())
    //    {
    //        foreach (GameObject section in sections)
    //        {
    //            section.SetActive(false);
    //        }
    //    }
    //}

    // --------------------------- InvokeOnSceneIndex ---------------------------
}
