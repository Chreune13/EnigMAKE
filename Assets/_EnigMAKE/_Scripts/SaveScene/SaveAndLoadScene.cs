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

        foreach (AutoRegisterSave save in autoRegisterSaves)
        {
            if (save.gameObject.name == "Jeton_actions(Clone)" || save.gameObject.name == "Jeton_enigmes(Clone)")
            {
                AutoRegisterSaveJetons saveJeton = save as AutoRegisterSaveJetons;
                saved.Add(JsonUtility.ToJson(saveJeton.GenerateSaved<DataSavedJetons>(), true));
            }
            else
            {
                saved.Add(JsonUtility.ToJson(save.GenerateSaved<DataSaved>(), true));
            }
        }

            /*if(save.gameObject.name== "Jeton_actions" || save.gameObject.name== "Jeton_enigmes")
            {
                foreach (AutoRegisterSaveJetons save in autoRegisterSaves)
                    saved.Add(JsonUtility.ToJson(save.GenerateSaved<DataSavedJetons>(), true));
            }
            else
            {
                foreach (AutoRegisterSave save in autoRegisterSaves)
                    saved.Add(JsonUtility.ToJson(save.GenerateSaved<DataSaved>(), true));
            }*/

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
            
           
                //foreach (AutoRegisterSave save in autoRegisterSaves)
                //{
                //    DataSaved data = JsonUtility.FromJson<DataSaved>(jsonArray[index]);
                //    save.GenerateLoaded(data);

                //    index++;
                //}

            foreach(string str in jsonArray)
            {
                if(jsonArray[index].Contains("Jeton_enigmes"))
                {
                    DataSavedJetons data = JsonUtility.FromJson<DataSavedJetons>(jsonArray[index]);

                    int jetonId = data.JetonID;

                    Vector3 jetonPosition = new Vector3(data.posX, data.posY, data.posZ);
                    Quaternion jetonRotation = new Quaternion(data.rotaX, data.rotaY, data.rotaZ, data.rotaW);
                    Vector3 jetonScale = new Vector3(data.scaleX, data.scaleY, data.scaleZ);

                    GameObject tr = new GameObject();
                    tr.transform.localPosition = jetonPosition;
                    tr.transform.localRotation = jetonRotation;
                    tr.transform.localScale = jetonScale;

                    EnigmeManager.instance.InstantiateEnigmeJetonFromExternal(jetonId, tr.transform);
                }
                else if (jsonArray[index].Contains("Jeton_actions"))
                {
                    DataSavedJetons data = JsonUtility.FromJson<DataSavedJetons>(jsonArray[index]);

                    int jetonId = data.JetonID;

                    Vector3 jetonPosition = new Vector3(data.posX, data.posY, data.posZ);
                    Quaternion jetonRotation = new Quaternion(data.rotaX, data.rotaY, data.rotaZ, data.rotaW);
                    Vector3 jetonScale = new Vector3(data.scaleX, data.scaleY, data.scaleZ);

                    GameObject tr = new GameObject();
                    tr.transform.localPosition = jetonPosition;
                    tr.transform.localRotation = jetonRotation;
                    tr.transform.localScale = jetonScale;

                    EnigmeManager.instance.InstantiateActionJetonFromExternal(jetonId, tr.transform);
                }
                else
                {
                    DataSaved data = JsonUtility.FromJson<DataSaved>(jsonArray[index]);
                    if (PrefabToSave.ContainsKey(data.PrefabName))
                    {
                        GameObject AutoRegisterObject = Instantiate(PrefabToSave[data.PrefabName]);

                        AutoRegisterObject.GetComponent<AutoRegisterSave>().GenerateLoaded<DataSaved>(data);
                    }
                }
                

                index++;

            }
                
        }
        else
        {
            Debug.Log("Le fichier n'existe pas...");
        }
    }
}
