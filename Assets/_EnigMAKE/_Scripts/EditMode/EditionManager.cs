using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EditionManager : MonoBehaviour
{

    [SerializeField]
    private Transform objectSpawnPoint;    
    
    [SerializeField]
    private Transform ground;


    public void SpawnPrefab(GameObject gameObject)
    {
        Instantiate(gameObject, transform.position, transform.rotation);
    }

    public void SpawnPrefabOnPlayer(GameObject gameObject)
    {
        Instantiate(gameObject, new Vector3(objectSpawnPoint.position.x, ground.position.y, objectSpawnPoint.position.z), objectSpawnPoint.rotation);
    }

    public void ScalePrefab(GameObject gameObject)
    {

    }
}
