using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientStart : MonoBehaviour
{
    public static ClientStart Singleton;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Multiple instance of ClientStart Singleton");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClientStartCoroutine());
    }

    IEnumerator ClientStartCoroutine()
    {
        while (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            yield return new WaitForSeconds(0.1f);

        if (NetworkManager.Singleton.IsClient)
        {
            if (SceneManagment.Singleton.playerState != PlayerType.GAMEMASTER)
            {
                if (SceneManagment.Singleton.sceneTheme == Theme.NOTHING)
                    NetworkGameManager.Singleton.BackToWaitingRoom();

                DecorsManager.Singleton.DisplayDecor(SceneManagment.Singleton.sceneTheme);
            }

        }
    }
}
