using UnityEngine;
using System.Collections;

public class Seals : MonoBehaviour
{
    void OnTriggerEnter()
    {

    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("There is a player inside the Seal Capture Zone");
        }
    }

    void OnTriggerExit()
    {

    }
}
