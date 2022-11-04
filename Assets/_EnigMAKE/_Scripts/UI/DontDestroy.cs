using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {

        Debug.Log("DontDestroy script disabled");
        //GameObject[] sceneManagment = GameObject.FindGameObjectsWithTag("SceneManagment");
        //GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        //if(this.gameObject.tag == "Player")
        //{
        //    if (player.Length > 1)
        //    {
        //        Destroy(this.gameObject);
        //    }

        //    DontDestroyOnLoad(this.gameObject);

        //} 

        //if (this.gameObject.tag == "SceneManagment")
        //{
        //    if (sceneManagment.Length > 1)
        //    {
        //        Destroy(this.gameObject);
        //    }

        //    DontDestroyOnLoad(this.gameObject);
        //}

    }
}
