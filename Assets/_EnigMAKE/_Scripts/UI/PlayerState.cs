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
        if (Singleton != null)
        {
            Debug.LogWarning("Multiple instance of PlayerStateSingleton");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;

        DontDestroyOnLoad(this.gameObject);
    }
}