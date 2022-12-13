using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRegisterSaveTransform : AutoRegisterSave
{
    public override void GenerateLoaded<T>(T saved)
    {
        //name = saved.prefab;
        transform.position = new Vector3(saved.posX,saved.posY,saved.posZ);
        transform.rotation = new Quaternion(saved.rotaX, saved.rotaY, saved.rotaZ, saved.rotaW);
        transform.localScale=new Vector3(saved.scaleX,saved.scaleY,saved.scaleZ);
    }

    public override T GenerateSaved<T>()
    {
        DataSaved saved = new DataSaved();

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

        return saved as T;
    }
}
