using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTestGameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Singleton?.SetGameMasterOrClientUIState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
