using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.Netcode;

[System.Serializable]
struct scenelist
{
    public int sceneId;

    public UnityEvent invokeDefaultMethod;
    public UnityEvent invokeGameMasterMethod;
    public UnityEvent invokePlayerMethod;
    public UnityEvent invokeEditionMethod;
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

    public void GetAndSetPlayerStateObject(int ps)
    {
        PlayerType playerType = (PlayerType)ps; 
        PlayerState.Singleton.playerState = playerType;
    }

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

                scenelists[i].invokeDefaultMethod.Invoke();

                if (PlayerState.Singleton.playerState == PlayerType.GAMEMASTER)
                {
                    scenelists[i].invokeGameMasterMethod.Invoke();
                }

                if (PlayerState.Singleton.playerState == PlayerType.PLAYER)
                {
                    scenelists[i].invokePlayerMethod.Invoke();
                }

                if (PlayerState.Singleton.playerState == PlayerType.EDIT)
                {
                    scenelists[i].invokeEditionMethod.Invoke();
                }

                //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenelists[i].sceneId);
            }

            //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenelists[i].sceneId);
        }
    }


    private int CheckSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    // --------------------------- InvokeOnSceneIndex ---------------------------
}
