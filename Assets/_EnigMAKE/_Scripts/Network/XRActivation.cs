using ParrelSync;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRActivation : MonoBehaviour
{
    public static XRActivation Singleton;

    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Multiple instance of PlayerStateSingleton");
            gameObject.SetActive(false);
            return;
        }

        Singleton = this;

        DontDestroyOnLoad(this.gameObject);

#if UNITY_EDITOR
        if (ClonesManager.IsClone())
        {
            string arg = ClonesManager.GetArgument();

            if (arg == "server")
            {
                UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StopSubsystems();

                UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
            else
            {
                UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

                UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }
        else
        {
            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
#endif
    }
}
