using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    private int score = 0;

    public static EnigmeManager instance;

    [SerializeField]
    private enigmlistelem[] enigmes;

    [SerializeField]
    private GameObject EnigmeJetonPrefab;
    [SerializeField]
    private GameObject ActionJetonPrefab;

    /*[SerializeField]
    private GameObject Spawner;*/

    [SerializeField]
    private int nextJetonID=1;


    private String scoreString_ = "Bravo ! Vous avez r�solu ";
   // [SerializeField]
    private GameObject scoreGo;
    private TMP_Text score_TMP;
    private String _scoreString = " �nigmes !";

    public void OnClick()
    {
        GameObject Spawner = TokenSpawnPointSingleton.instance.gameObject;

        InstantiateEnigmeJetonFromExternal(nextJetonID, Spawner.transform);
        InstantiateActionJetonFromExternal(0, Spawner.transform);

        nextJetonID++;
    }

    public void InstantiateEnigmeJetonFromExternal(int jetonId, Transform JetonEnigmePosition)
    {
        EnigmesClassement enigmesClassement = EnigmeJetonPrefab.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI enigme_textMeshProUGUI = EnigmeJetonPrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        enigmesClassement.SetID(jetonId);

        enigme_textMeshProUGUI.text = "E" + enigmesClassement.GetID().ToString();

        Instantiate(EnigmeJetonPrefab, JetonEnigmePosition);

        if(jetonId > nextJetonID)
            nextJetonID = jetonId;
    }

    public void InstantiateActionJetonFromExternal(int jetonId, Transform JetonActionPosition)
    {
        EnigmesClassement actionsClassement = ActionJetonPrefab.GetComponentInChildren<EnigmesClassement>();
        EnigmesClassement enigmesClassement = EnigmeJetonPrefab.GetComponentInChildren<EnigmesClassement>();
        TMPro.TextMeshProUGUI action_textMeshProUGUI = ActionJetonPrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        actionsClassement.SetID(jetonId);

        action_textMeshProUGUI.text = "A" + enigmesClassement.GetID().ToString();

        Instantiate(ActionJetonPrefab, JetonActionPosition);

        if (jetonId > nextJetonID)
            nextJetonID = jetonId;
    }

    private void Awake()
    {
        
        if (instance != null)
        {
            Debug.LogError("Multiple instance of singleton EnigmeManager!");
            return;
        }
        enigmes = new enigmlistelem[3];
        for(int i = 0; i < enigmes.Length; i++)
        {
            enigmes[i].a = new actions[1];
        }
        instance = this;

        if(ScorePanelSingleton.instance)
        {
            scoreGo = ScorePanelSingleton.instance.gameObject;
            score_TMP = scoreGo.transform.GetChild(2).GetComponent<TMP_Text>();
            scoreGo.SetActive(false);
            Debug.Log("awake " + score_TMP.text);
        }
    }

    public void SetEnigmElem(Enigme enigme)
    {

        enigmes[nextJetonID - 1].reff_e = enigme;

        Debug.Log(enigmes[nextJetonID - 1].reff_e);
    }
    public void SetActionElem(Enigme action,int id)
    {
        Debug.Log(nextJetonID + "  " + id);
        
        enigmes[nextJetonID - 1].a[id].reff_a = action;
        Debug.Log(enigmes[nextJetonID - 1].a[id].reff_a);
        enigmes[nextJetonID - 1].a[id].actionId = nextJetonID;
        
        
    }

    public void goToNext(int id)
    {
        for (int i = 0; i < enigmes.Length; i++)
        {
            if (enigmes[i].reff_e.getID() == id)
            {

                for (int j = 0; j < enigmes[i].a.Length; j++)
                {
                    enigmes[i].a[j].reff_a.ExecuteAction();
                }

                break;
            }
        }

        //GameObject score = ScorePanelSingleton.instance.gameObject;
        scoreGo.SetActive(true);
        score_TMP.text = scoreString_ + GetScore().ToString() + _scoreString;
        Debug.Log("goToNext " + score_TMP.text);

    }

    public void SetJetonID(int id)
    {
        nextJetonID = id;
    }
    public int GetJetonID()
    {
        return nextJetonID;
    }
    public void IncrementScore()
    {
        score++;
    }

    public int GetScore()
    {
        return score;
    }

}
