using UnityEngine;
using TMPro;
public class EnigmesClassement : MonoBehaviour
{
    [SerializeField]
    private int ID = 0;
    private Enigme enigme = new Enigme();
    private Enigme action = new Enigme();

    [SerializeField]
    private bool trigger = false;
    public void SetID(int id)
    {
        this.ID = id;
    }
    public int GetID()
    {
        return ID;
    }
    private void Awake()
    {
        ID = ID + 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chest" || other.tag == "Locke" || other.tag == "Door" || other.tag == "Digicode" || other.tag == "Breakable")
        {
            if (gameObject.tag == "Enigmes" && (other.gameObject.tag != "Enigmes" || other.gameObject.tag != "Actions" /*|| other.gameObject.tag != "Key" )*/))
            {
                
                enigme = other.GetComponent<Enigme>();
                transform.parent.parent.SetParent(other.transform);
                trigger = true;
            }
            if (gameObject.tag == "Actions" && (other.gameObject.tag != "Enigmes" || other.gameObject.tag != "Actions" /*|| other.gameObject.tag != "Key"*/))
            {
                action = other.GetComponent<Enigme>();
                transform.parent.parent.SetParent(other.transform);
                trigger = true;
            }

            
            
            if(trigger)
            {
                if (gameObject.tag == "Enigmes" && (other.gameObject.tag != "Enigmes" || other.gameObject.tag != "Actions"))
                {
                    EnigmeManager.instance.SetEnigmElem(enigme);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    for(int i = 0;i<transform.childCount;i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                if (gameObject.tag == "Actions" && (other.gameObject.tag != "Enigmes" || other.gameObject.tag != "Actions"))
                {
                    EnigmeManager.instance.SetActionElem(action, ID - 1); //enigme.getID()
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                trigger =false;
            }
            
            
        }
        Debug.Log(ID);

    }
    public Enigme GetEnigme()
    {
        return enigme;
    }
    public bool IsTrigger()
    {
        return trigger;
    }

    public void SetTrigger(bool trig)
    {
        trigger = trig;
    }
}
