using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
struct enigmlistelem
{
    public Enigme reff;
    public actions[] actions;
}
[System.Serializable]
struct actions
{
    public Enigme reff;
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

    private int JetonID=0;

    public void OnClick()
    {
        EnigmesClassement enigmesClassement = EnigmeJeton.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI enigme_textMeshProUGUI = EnigmeJeton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        EnigmesClassement actionsClassement = ActionJeton.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI action_textMeshProUGUI = ActionJeton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        enigmesClassement.SetID(JetonID);
        actionsClassement.SetID(JetonID);

        enigme_textMeshProUGUI.text = "E"+enigmesClassement.GetID().ToString();
        action_textMeshProUGUI.text = "A" + actionsClassement.GetID().ToString();

        Instantiate(EnigmeJeton);
        Instantiate(ActionJeton);   

        JetonID++;
    }
    private void Awake()
    {
        
        if (instance != null)
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
            if(enigmes[i].reff.getID()==id)
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
