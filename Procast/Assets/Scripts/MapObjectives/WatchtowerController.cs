using UnityEngine;
using System.Collections;

public class WatchtowerController : MonoBehaviour {

    bool beingCaptured;
    bool effectActive;
    public GameObject capZone;

    void Start()
    {

    }

    void OnTriggerStay()
    {
        beingCaptured = true;
        Debug.Log("Watchtower Being Captured");
    }
}
