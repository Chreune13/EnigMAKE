using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {

        //Debug.Log("DontDestroy script disabled");
        GameObject[] player = GameObject.FindGameObjectsWithTag("ClientOnly");

        if (this.gameObject.tag == "ClientOnly")
        {
            if (player.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

        }

            DontDestroyOnLoad(this.gameObject);
    }
}

