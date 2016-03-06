using UnityEngine;
using System.Collections;

public class KillMe : MonoBehaviour
{
    public GameObject deadBody;

    void Awake()
    {
        Destroy(deadBody, 5f);
    }
}
