using UnityEngine;

public class RecycleItem : MonoBehaviour
{   //only for the recycle items to keep it's value
    public int value = 2;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RecycleItem"))
        {
            Level2Manager.Instance.AddMoney(value);
            Destroy(gameObject);
        }
    }
    

    
    public void Collect()
    {
        //change this to when put the object onto the scale
        //Level2Manager.Instance.AddMoney(value);
        //Destroy(gameObject);
    }
    */
}
