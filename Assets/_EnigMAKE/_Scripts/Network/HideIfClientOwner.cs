using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HideIfClientOwner : NetworkBehaviour
{
    [SerializeField]
    MeshRenderer[] Renderers;

    [SerializeField]
    SkinnedMeshRenderer[] SkinnedRenderers;

    private void Awake()
    {
        StartCoroutine(HideMeshRendererCoroutine());
    }

    IEnumerator HideMeshRendererCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            if (IsClient && IsOwner)
                break;

            else if (IsServer)
                yield return null;

            else if (IsClient && !IsOwner)
                yield return null;
        }

        foreach(MeshRenderer renderer in Renderers)
        {
            renderer.enabled = false;
        }

        foreach (SkinnedMeshRenderer renderer in SkinnedRenderers)
        {
            renderer.enabled = false;
        }

        yield return null;
    }
}
