using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Singleton;

    public PlayerType playerState;

    // Start is called before the first frame update
    void Awake()
    {
        /*GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerState");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }*/

        if (Singleton != null)
        {
            Debug.LogError("Multiple instance of PlayerStateSingleton");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
