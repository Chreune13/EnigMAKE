using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditionManager : MonoBehaviour
{
    public void SpawnPrefab(GameObject gameObject)
    {
        Instantiate(gameObject, transform.position, transform.rotation);
    }

    public void SpawnPrefabOnPlayer(GameObject player, GameObject gameObject)
    {
        Instantiate(gameObject, player.transform.position + new Vector3Int(0,0,1), transform.rotation);
    }

    public void ScalePrefab(GameObject gameObject)
    {

    }

}
