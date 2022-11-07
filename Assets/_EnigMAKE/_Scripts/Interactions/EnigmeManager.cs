using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
struct enigmlistelem
{
    public Enigme reff;
    public actions[] a;
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
    private List<enigmlistelem> enigmes;

    //private enigmlistelem enigme;


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

    public void SetEnigmElem()
    {
        enigmes = new List<enigmlistelem>();

        if (EnigmeJeton.GetComponentInChildren<EnigmesClassement>().IsTrigger() == true && ActionJeton.GetComponentInChildren<EnigmesClassement>().IsTrigger() == true)
        {
            print("présent");
            /*enigme.reff = EnigmeJeton.GetComponentInChildren<EnigmesClassement>().GetEnigme();
            for (int i = 0; i < 3; i++)
            {
                enigme.a[i].reff = ActionJeton.GetComponentInChildren<EnigmesClassement>().GetEnigme();
                enigme.a[i].actionId = JetonID;


            }*/
            enigmes.Add(new enigmlistelem() { reff = EnigmeJeton.GetComponentInChildren<EnigmesClassement>().GetEnigme(),a=new actions[3] });
        }
        

    }

    public void goToNext(int id)
    {
        
        
        for (int i = 0; i < enigmes.Count; i++)
        {
            if (enigmes[i].reff.getID() == id)
            {

                for (int j = 0; j < enigmes[i].a.Length; j++)
                {
                    enigmes[i].a[j].reff.ExecuteAction(enigmes[i].a[j].actionId);
                }

                break;
            }
        }
        
       
    }

}
