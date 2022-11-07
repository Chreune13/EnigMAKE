using UnityEngine;

public class EnigmesClassement : MonoBehaviour
{
    private int ID = 0;
    private Enigme enigme;
    private bool trigger = false;
    public Enigme GetEnigme()
    {
        return enigme;
    }

    public void SetID(int id)
    {
        this.ID = id;
    }
    public int GetID()
    {
        return ID;
    }

    public bool IsTrigger()
    {
        return trigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged")
        {
            transform.parent.parent.SetParent(other.transform);
            enigme = other.GetComponent<Enigme>();
            trigger = true;
            EnigmeManager.instance.SetEnigmElem();
        }


    }

}
