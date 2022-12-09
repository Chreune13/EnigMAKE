using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loop());
    }

    IEnumerator loop()
    {
        while(!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            yield return new WaitForSeconds(.1f);

        if(NetworkManager.Singleton.IsServer)
        {
            if (gm)
            {
                GameObject spawned = Instantiate(gm);
                spawned.transform.position = new Vector3(1.25f, 1f, 1f);
                spawned.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
