using UnityEngine;
using System.Collections;

public class CasterCam : MonoBehaviour
{
    public Texture2D crosshairTex;
    private Rect crhPosition;

    float xChange;
    bool rShoulder;

    void Start()
    {
        rShoulder = true;
    }

    void Update()
    {
        //RayCasting();
        CrosshairPosition();
    }

    void LateUpdate()
    {
        //CamLogic();
    }

    void OnGUI()
    {
        GUI.DrawTexture(crhPosition, crosshairTex);
    }

    void RayCasting()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000f;
        Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;
        Ray aimingRay = new Ray(transform.position, forward);

        if (Physics.Raycast(aimingRay, out hit))
        {
            Vector3 hitPoint = hit.point;
            Debug.Log(hit.collider.tag);
            Debug.Log(hitPoint);
        }
    }
    void CrosshairPosition()
    {
        crhPosition = new Rect((Screen.width - crosshairTex.width) / 2,
           (Screen.height - crosshairTex.height) / 2,
            crosshairTex.width, crosshairTex.height); //determines width/height of our crosshair GUI texture
    }

    void CamLogic()
    {
        //Toggles cam location between left and right over the shoulder.
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (rShoulder == true)
            {
                xChange = -2;
                rShoulder = false;
                Debug.Log(rShoulder);
                Debug.Log(xChange);
            }
            else
            {
                xChange = 2;
                rShoulder = true;
                Debug.Log(rShoulder);
                Debug.Log(xChange);
            }
        }
    }


}


