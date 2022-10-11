using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerUI : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LocalNetworkManager.Singleton?.LocalPlayerIsSpawned();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
