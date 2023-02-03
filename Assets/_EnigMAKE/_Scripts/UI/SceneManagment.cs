using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.Netcode;

[System.Serializable]
struct sceneMetaData
{
    //public int sceneId;
    public string sceneName;
    public Theme sceneTheme;

    public UnityEvent invokeDefaultMethod;
    public UnityEvent invokeGameMasterMethod;
    public UnityEvent invokePlayerMethod;
    public UnityEvent invokeEditionMethod;
}

public enum PlayerType
{
    GAMEMASTER = 0,
    PLAYER = 1,
    EDIT = 2
}

public enum Theme
{
    MEDIEVAL,
    SCIENCE,
    CHAMBRE,
    NOTHING
}

public class SceneManagment : MonoBehaviour
{
    public static SceneManagment Singleton;

    public PlayerType playerState;

    private GameObject teleporter;
    private GameObject player;
    public Theme sceneTheme;

    void Awake()
    {
        sceneTheme = Theme.NOTHING;

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
        if(sceneName == "EnigMakeWaitingRoom")
        {
            BackToMenu();
        }

        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        GetComponent<SaveAndLoadScene>().autoRegisterSaves.Clear();
        Scene scn = SceneManager.GetActiveScene();
      
      //  foreach( GameObject gameObject in scn.GetRootGameObjects())
        SceneManager.LoadScene(scn.name);
    }
    private void BackToMenu()
    {
        if (NetworkManagerSingleton.instance)
        {
            Destroy(NetworkManagerSingleton.instance.gameObject);
            sceneTheme = Theme.NOTHING;
        }
    }

    private void ThemeToLoad(sceneMetaData scene)
    {
        if (DecorsManager.Singleton)
            DecorsManager.Singleton.DisplayDecor(scene.sceneTheme);
    }

    // --------------------------------------------------------------------------



    // --------------------------- InvokeOnSceneIndex ---------------------------


    [SerializeField]
    private sceneMetaData[] scenes;
    
    public void SetPlayerState(int ps)
    {
        playerState = (PlayerType)ps;
    }

    public void SetThemeState(int ts)
    {
        sceneTheme = (Theme)ts;
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
        
        if (scenes == null)
            return;

        for (int i = 0; i < scenes.Length; i++)
        {
            if (GetSceneIndexFromName(scenes[i].sceneName) == GetCurrentSceneIndex())
            {

                scenes[i].invokeDefaultMethod.Invoke();

                if (playerState == PlayerType.GAMEMASTER)
                {
                    scenes[i].invokeGameMasterMethod.Invoke();
                }

                if (playerState == PlayerType.PLAYER)
                {
                    scenes[i].invokePlayerMethod.Invoke();
                }

                if (playerState == PlayerType.EDIT)
                {
                    scenes[i].invokeEditionMethod.Invoke();
                }

                scenes[i].sceneTheme = sceneTheme;
                ThemeToLoad(scenes[i]);
                Debug.Log(sceneTheme);
                //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenes[i].sceneId);
            }

            //Debug.Log("Current sceneId : " + id + " sceneId list : " + scenes[i].sceneId);
        }

        teleporter = GetTeleporter();
        player = GetPlayer();

        player.transform.position = new Vector3(teleporter.transform.position.x, 
                                                player.transform.position.y, 
                                                teleporter.transform.position.z);
    }


    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private int GetSceneIndexFromName(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    // --------------------------------------------------------------------------


    // -------------------------- GetTeleporter ---------------------------------

    private GameObject GetTeleporter()
    {
        return GameObject.FindGameObjectWithTag("Teleporter"); ;
    }
    private GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    // --------------------------------------------------------------------------
}
