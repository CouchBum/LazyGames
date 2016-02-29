using UnityEngine;
using System.Collections;

public class HeadController : MonoBehaviour
{
    protected float vertLookSpeed = -500;

    void Start()
    {
    }
    void Update()
    {
        HeadLook();
    }

    void HeadLook()
    {
        float v = vertLookSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        transform.Rotate(v, 0, 0);
    }
}
