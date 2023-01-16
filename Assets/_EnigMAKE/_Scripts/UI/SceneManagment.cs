using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.Netcode;

[System.Serializable]
struct scenelist
{
    //public int sceneId;
    public string sceneName;

    public UnityEvent invokeDefaultMethod;
    public UnityEvent invokeGameMasterMethod;
    public UnityEvent invokePlayerMethod;
    public UnityEvent invokeEditionMethod;
}


public class SceneManagment : MonoBehaviour
{
    public static SceneManagment Singleton;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Multiple instance of SceneManagementSingleton");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // --------------------------- Loading Methods ---------------------------

    public void SceneToLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        GetComponent<SaveAndLoadScene>().autoRegisterSaves.Clear();
        Scene scn = SceneManager.GetActiveScene();
      
      //  foreach( GameObject gameObject in scn.GetRootGameObjects())
        SceneManager.LoadScene(scn.name);
    }

    // --------------------------------------------------------------------------



    // --------------------------- InvokeOnSceneIndex ---------------------------


    [SerializeField]
    private scenelist[] scenelists;
    
    /// <summary>
    /// GameMaster = 0
    /// Player = 1
    /// Edit mode = 2
    /// </summary>
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

        for (int i = 0; i < scenelists.Length; i++)
        {
            if (GetSceneIndexFromName(scenelists[i].sceneName) == GetCurrentSceneIndex())
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


    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private int GetSceneIndexFromName(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    // --------------------------- InvokeOnSceneIndex ---------------------------
}
