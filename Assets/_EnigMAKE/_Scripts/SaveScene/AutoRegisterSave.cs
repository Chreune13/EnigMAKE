using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoRegisterSave : MonoBehaviour
{
    private void Start()
    {
        SaveAndLoadScene.Singleton.autoRegisterSaves.Add(this);
    }

    abstract public T GenerateSaved<T>() where T : DataSaved;
    abstract public void GenerateLoaded<T>(T saved) where T : DataSaved;
}
