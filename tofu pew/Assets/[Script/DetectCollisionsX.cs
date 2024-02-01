using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionsX : MonoBehaviour
{

    // all this for test use
    private void OnTriggerEnter(Collider other)
    {
        //this one for test use 
        if (other.CompareTag("Enemy"))
        {
            // Handle the collection or destruction of the enemy tank
            Destroy(other.gameObject); // Destroy the enemy tank when collected\
            Destroy(gameObject);
        }

        //team_1_collection
        //if (other.CompareTag("Team_1"))
        //{
        //    // Handle the collection or destruction of the enemy tank
        //    Destroy(other.gameObject); // Destroy the enemy tank when collected\
        //    Destroy(gameObject);
        //}
        ////team_2_collection
        //if (other.CompareTag("Team_2"))
        //{
        //    // Handle the collection or destruction of the enemy tank
        //    Destroy(other.gameObject); // Destroy the enemy tank when collected\
        //    Destroy(gameObject);
        //}
        //if (other.CompareTag("boundary"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
