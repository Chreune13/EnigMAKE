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
            gameObject.SetActive(false);
            return;
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

    public void Save(Theme sceneTheme)
    {
        string dir = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        List<string> saved = new List<string>();

        if (autoRegisterSaves == null)
        {
            autoRegisterSaves.Clear();
        }

        foreach (AutoRegisterSave save in autoRegisterSaves)
        {

            saved.Add(JsonUtility.ToJson(save.GenerateSaved<DataSaved>(), true));

        }

        string savedList = "";

        for (int i = 0; i < saved.Count; i++)
        {
            savedList += saved[i];

            if (i != saved.Count - 1)
                savedList += "\n;\n";
        }

        File.WriteAllText(dir + "/" + SceneManagment.Singleton.sceneSaveFiles[(int)sceneTheme], savedList);
    }

    public static string NormalizeName(string originalName)
    {
        string name = originalName;

        if (name.Contains(" ("))
        {
            string[] tmp = name.Split(" (");
            name = tmp[0];
        }

        return name;
    }

    public void Load(Theme sceneTheme)
    {
        string saveFilePath = Application.persistentDataPath + "/Saves/" + SceneManagment.Singleton.sceneSaveFiles[(int)sceneTheme];

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            string[] jsonArray = json.Split(';');
            int index = 0;

            int IDmax = -1;
            if (autoRegisterSaves != null)
            {
                foreach (AutoRegisterSave reg in autoRegisterSaves)
                {
                    Destroy(reg.gameObject);
                }
                autoRegisterSaves.Clear();
            }

            foreach (string str in jsonArray)
            {
                int jetonId = 0;
                DataSaved data = JsonUtility.FromJson<DataSaved>(jsonArray[index]);
                if (data != null)
                {
                    jetonId = data.enigmeID;
                    string normName = NormalizeName(data.PrefabName);
                    if (PrefabToSave.ContainsKey(normName))
                    {
                        data.PrefabName = normName;
                        GameObject AutoRegisterObject = Instantiate(PrefabToSave[data.PrefabName], new Vector3(data.posX, data.posY, data.posZ), new Quaternion(data.rotaX, data.rotaY, data.rotaZ, data.rotaW));

                        AutoRegisterObject.transform.position = new Vector3(data.posX, data.posY, data.posZ);

                        AutoRegisterObject.GetComponent<AutoRegisterSave>().GenerateLoaded<DataSaved>(data);
                    }
                    if (IDmax < jetonId)
                        IDmax = jetonId;
                    index++;
                }   
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
