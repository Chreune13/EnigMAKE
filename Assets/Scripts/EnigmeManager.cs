using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct enigmListElem
{
    public Enigme reff;
    public Actions[] actions;

}

[System.Serializable]
struct Actions
{
    public Enigme reff;

    public int actionId;
}

public class EnigmeManager : MonoBehaviour
{
    public static EnigmeManager instance;

    [SerializeField]
    private enigmListElem[] enigmes;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple instance of singleton EnigmeManager!");
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        

        //for (int i = 1; i < enigmes.Length; i++)
        //{
          
        //   enigmes[i].reff.enabled = false; 
        //}
            

    }

    public void goToNext(int id)
    {
        for (int i = 0; i < enigmes.Length; i++)
        {
            if(enigmes[i].reff.ID==id)
            {
                for (int j = 0; j < enigmes[i].actions.Length; j++)
                {
                    enigmes[i].actions[j].reff.ExecuteAction(enigmes[i].actions[j].actionId);
                }

                break;
            }
        }
    }

}
