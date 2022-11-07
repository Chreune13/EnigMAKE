using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandNetworkController : NetworkBehaviour
{
    [SerializeField]
    GameObject LeftHandPrefab;

    [SerializeField]
    GameObject RightHandPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(IsOwner)
        {
            if (LeftHandPrefab)
            {
                GameObject LeftHand = Instantiate(LeftHandPrefab);

                LeftHand.transform.parent = transform;
            }

            if (RightHandPrefab)
            {
                GameObject RightHand = Instantiate(RightHandPrefab);

                RightHand.transform.parent = transform;
            }
        }
    }
}
