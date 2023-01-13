using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndLoadScene : MonoBehaviour
{
    [SerializeField]
    GameObject[] Prefab;

    Dictionary<string, GameObject> PrefabToSave = new Dictionary<string, GameObject>();

    public static SaveAndLoadScene Singleton;
        
    public List<AutoRegisterSave> autoRegisterSaves = new List<AutoRegisterSave>();
    private void Awake()
    {
        if(!Singleton)
            Singleton = this;
        else
        {
            Debug.LogWarning("Multiple instance of type SaveAndLoadScene");
        }

       
        if(Prefab.Length > 0)
        {
            for(int i = 0; i < Prefab.Length; i++)
            {
                string name = NormalizeName(Prefab[i].name);
                    
                PrefabToSave.Add(name,Prefab[i]);
            }
        }
    }
    public void Save()
    {
        string dir = Application.persistentDataPath + "/Saves";
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        List<string> saved = new List<string>();

        //if (autoRegisterSaves != null)
        //{
        //    autoRegisterSaves.Clear();
        //}

        foreach (AutoRegisterSave save in autoRegisterSaves)
        {
           
           saved.Add(JsonUtility.ToJson(save.GenerateSaved<DataSaved>(), true));
           
        }

        string savedList = "";

        for(int i = 0; i < saved.Count; i++)
        {
            savedList += saved[i];

            if (i != saved.Count - 1)
                savedList += "\n;\n";
        }

        File.WriteAllText(dir + "/save1.txt", savedList);
    }

    public static string NormalizeName(string originalName)
    {
        string name = originalName;

        if (name.Contains("(Clone)"))
        {
            string[] tmp = name.Split("(Clone)");
            name = tmp[0];
        }

        return name;
    }

    public void Load()
    {
        string saveFilePath = Application.persistentDataPath + "/Saves/save1.txt";
        
        if (File.Exists(saveFilePath))
        {
            
           
            string json= File.ReadAllText(saveFilePath);
            string[] jsonArray = json.Split(';');
            int index = 0;

            int IDmax=-1;
            foreach (string str in jsonArray)
            {
                
                DataSaved data = JsonUtility.FromJson<DataSaved>(jsonArray[index]);
                int jetonId = data.enigmeID;

                if (PrefabToSave.ContainsKey(data.PrefabName))
                {
                    if(autoRegisterSaves != null)
                    {
                        autoRegisterSaves.Clear();
                    }
                    GameObject AutoRegisterObject = Instantiate(PrefabToSave[data.PrefabName]);

                    AutoRegisterObject.GetComponent<AutoRegisterSave>().GenerateLoaded<DataSaved>(data);
                }
                if (IDmax < jetonId)
                    IDmax = jetonId;
                index++;
            }
           
            for (int i = 0; i < IDmax; i++)
            {
                EnigmeManager.instance.OnClick();
            }

        }
        else
        {
            Debug.Log("Le fichier n'existe pas...");
        }
    }
}
