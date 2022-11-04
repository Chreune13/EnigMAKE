using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnigmesClassement : MonoBehaviour
{
    private int ID=0;

    public void SetID(int id)
    { 
        this.ID = id;
    }
    public int GetID()
    {
        return ID;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }


}
