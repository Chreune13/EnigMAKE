using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public void SpawnPrefab(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation) ;
    }
}
