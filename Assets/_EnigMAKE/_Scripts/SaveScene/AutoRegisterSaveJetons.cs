using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRegisterSaveJetons : AutoRegisterSave
{
    [SerializeField]
    private EnigmeManager enigmeManager;
    public override void GenerateLoaded<T>(T saved)
    {
        if(typeof(T) == typeof(DataSavedJetons))
            GenerateLoadedJetons(saved as DataSavedJetons);
    }

    public override T GenerateSaved<T>()
    {
        if (typeof(T) == typeof(DataSavedJetons))
            return GenerateSavedJeton() as T;

        return null;
    }

    void GenerateLoadedJetons(DataSavedJetons saved)
    {
        transform.position = new Vector3(saved.posX, saved.posY, saved.posZ);
        transform.rotation = new Quaternion(saved.rotaX, saved.rotaY, saved.rotaZ, saved.rotaW);
        transform.localScale = new Vector3(saved.scaleX, saved.scaleY, saved.scaleZ);

        enigmeManager.SetJetonID(saved.JetonID);
    }

    DataSavedJetons GenerateSavedJeton()
    {
        DataSavedJetons saved = new DataSavedJetons();

        saved.posX = transform.position.x;
        saved.posY = transform.position.y;
        saved.posZ = transform.position.z;
        saved.rotaX = transform.rotation.x;
        saved.rotaY = transform.rotation.y;
        saved.rotaZ = transform.rotation.z;
        saved.rotaW = transform.rotation.w;
        saved.scaleX = transform.localScale.x;
        saved.scaleY = transform.localScale.y;
        saved.scaleZ = transform.localScale.z;

        saved.PrefabName = SaveAndLoadScene.NormalizeName(name);

        saved.JetonID = enigmeManager.GetJetonID();

        return saved;
    }
}
