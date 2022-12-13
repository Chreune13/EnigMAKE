using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
struct enigmlistelem
{
    public Enigme reff_e;
    public actions[] a;
}
[System.Serializable]
struct actions
{
    public Enigme reff_a;
    public int actionId;
}

public class EnigmeManager : MonoBehaviour
{
    public static EnigmeManager instance;

    [SerializeField]
    private enigmlistelem[] enigmes;

    [SerializeField]
    private GameObject EnigmeJeton;
    [SerializeField]
    private GameObject ActionJeton;

    [SerializeField]
    private GameObject Spawner;

    [SerializeField]
    private int JetonID=0;

    private EnigmesClassement enigmesClassement;
    private EnigmesClassement actionsClassement;
    public void OnClick()
    {
        enigmesClassement = EnigmeJeton.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI enigme_textMeshProUGUI = EnigmeJeton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        actionsClassement = ActionJeton.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI action_textMeshProUGUI = ActionJeton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        enigmesClassement.SetID(JetonID);
        actionsClassement.SetID(JetonID);

        enigme_textMeshProUGUI.text = "E" + enigmesClassement.GetID().ToString();
        action_textMeshProUGUI.text = "A" + actionsClassement.GetID().ToString();

        Instantiate(EnigmeJeton, Spawner.transform);
        Instantiate(ActionJeton, Spawner.transform);

        JetonID++;
        
       
    } 
    
    private void Awake()
    {
        
        if (instance != null)
        {
            Debug.LogError("Multiple instance of singleton EnigmeManager!");
            return;
        }
        enigmes = new enigmlistelem[5];
        for(int i = 0; i < enigmes.Length; i++)
        {
            enigmes[i].a = new actions[3];
        }
        instance = this;
    }
    public void SetEnigmElem(Enigme enigme)
    {

        enigmes[JetonID-1].reff_e = enigme;

        Debug.Log(enigmes[JetonID - 1].reff_e);


    }
    public void SetActionElem(Enigme action,int id)
    {
        
        
            enigmes[JetonID - 1].a[id].reff_a = action;
            Debug.Log(enigmes[JetonID - 1].a[id].reff_a);
            enigmes[JetonID - 1].a[id].actionId = JetonID;
        
        
    }

    public void goToNext(int id)
    {
        
        
      
            for (int i = 0; i < enigmes.Length; i++)
            {
                if (enigmes[i].reff_e.getID() == id)
                {

                    for (int j = 0; j < enigmes[i].a.Length; j++)
                    {
                        enigmes[i].a[j].reff_a.ExecuteAction(enigmes[i].a[j].actionId);
                    }

                    break;
                }
            }
      
        
        
       
    }

    public void SetJetonID(int id)
    {
        JetonID = id+1;
    }
    public int GetJetonID()
    {
        return JetonID+1;
    }

}
