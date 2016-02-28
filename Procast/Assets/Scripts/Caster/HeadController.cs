using UnityEngine;
using System.Collections;

public class HeadController : MonoBehaviour
{
    protected float vertLookSpeed = -1.0f;

    void Start()
    {
    }
    void Update()
    {
        HeadLook();
        //RayCasting(); used to have this function come directly from the caster's head
    }

    void HeadLook()
    {
        float v = vertLookSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, 0, 0);
    }
}
