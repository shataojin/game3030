using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private bool GetItem = false;
    public int randomNum;
    void Start()
    {

    }


    void Update()
    {
        if (GetItem == true && Input.GetKeyDown(KeyCode.H))
        {
            UseItem();
            GetItem = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GetItem = true;
            // Destroy this pickup item
            Destroy(other.gameObject);

            Debug.Log("item is destroy");

            randomNum = Random.Range(0, 3);//what type of power you get

            Debug.Log("item type of poweris : "+ randomNum);
        }
    }

    private void UseItem()
    {
        //call the power type when  you use  exple: if(randomNum==1){....} i guess
        Debug.Log("item is used");
    }
}
