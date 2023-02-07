using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorsManager : MonoBehaviour
{

    public static DecorsManager Singleton;

    [SerializeField]
    private GameObject[] decors;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Multiple instance of DecorsManager");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;
    }

    public void DisplayDecor(Theme id)
    {
        /*for(int i = 0; i < decors.Length; i++)
        {
            if(i == (int)id)
            {
                decors[i].gameObject.SetActive(true);
            } else
            {
                decors[i].gameObject.SetActive(false);
            }
        }*/
    }

}
