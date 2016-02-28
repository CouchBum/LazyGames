using UnityEngine;
using System.Collections;

public class ProjectileHandler : MonoBehaviour {

	void Start ()
    {
	
	}

    void Update()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter (Collision col)
    {
        Destroy(gameObject);
    }
}
