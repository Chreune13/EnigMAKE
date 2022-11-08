using UnityEngine;

public class EnigmesClassement : MonoBehaviour
{
    private int ID = 0;
    private Enigme enigme=new Enigme();
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
        print("OnTriggerEnter");
        if (other.tag != "Untagged")
        {
            print("Untagged");
            if(gameObject.tag=="Enigmes")
                enigme = other.GetComponent<Enigme>();
            if(gameObject.tag=="Actions")
                action = other.GetComponent<Enigme>();

            transform.parent.parent.SetParent(other.transform);
            trigger=true;
            if(trigger)
            {
                if (gameObject.tag == "Enigmes")
                    EnigmeManager.instance.SetEnigmElem(enigme);
                if (gameObject.tag == "Actions")
                    EnigmeManager.instance.SetActionElem(action,ID-1);
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
