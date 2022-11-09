using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnableIfClient : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjectToEnable;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetectNetworkStateCoroutine());
    }

    IEnumerator DetectNetworkStateCoroutine()
    {
        while (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            yield return new WaitForSeconds(0.1f);

        if(NetworkManager.Singleton.IsClient)
        {
            foreach(GameObject toEnable in gameObjectToEnable)
            {
                if(toEnable)
                {
                    toEnable.SetActive(true);
                }
            }
        }
    }
}
