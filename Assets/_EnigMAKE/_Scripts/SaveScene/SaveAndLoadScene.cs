using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndLoadScene : MonoBehaviour
{
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
    }
    public void Save()
    {
        string dir = Application.persistentDataPath + "/Saves";
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        List<string> saved = new List<string>();

        foreach(AutoRegisterSave save in autoRegisterSaves)
        {
           saved.Add(JsonUtility.ToJson(save.GenerateSaved(), true));
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

    public void Load()
    {
        string saveFilePath = Application.persistentDataPath + "/Saves/save1.txt";
        //DataSaved[] data;
        if (File.Exists(saveFilePath))
        {
            
           
            string json= File.ReadAllText(saveFilePath);
            string[] jsonArray = json.Split(';');
            int index = 0;
            if(autoRegisterSaves.Count == jsonArray.Length)
            {
                foreach (AutoRegisterSave save in autoRegisterSaves)
                {
                    DataSaved data = JsonUtility.FromJson<DataSaved>(jsonArray[index]);
                    save.GenerateLoaded(data);

                    index++;
                }
            }
            else
            {
                Debug.LogError("Not the same length");
            }
                
        }
        else
        {
            Debug.Log("Le fichier n'existe pas...");
        }
    }
}
