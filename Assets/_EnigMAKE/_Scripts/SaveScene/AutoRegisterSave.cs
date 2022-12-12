using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoRegisterSave : MonoBehaviour
{
    private void Start()
    {
        SaveAndLoadScene.Singleton.autoRegisterSaves.Add(this);
    }

    abstract public DataSaved GenerateSaved();
    abstract public void GenerateLoaded(DataSaved saved);
}
