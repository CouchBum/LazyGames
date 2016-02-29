using UnityEngine;
using System.Collections;

public class CasterCam : MonoBehaviour
{
    public Texture2D crosshairTex;
    private Rect crhPosition;
    private float csTextH;
    private float csTextW;
    private float sizeAdjuster = .10f;
    bool rShoulder;

    void Start()
    {
        rShoulder = true;
    }

    void Update()
    {
        CrosshairPosition();
    }

    void LateUpdate()
    {
        CamLogic();
    }

    void OnGUI()
    {
        GUI.DrawTexture(crhPosition, crosshairTex);
    }

    void CrosshairPosition()
    {
        csTextH = crosshairTex.height * sizeAdjuster;
        csTextW = crosshairTex.width * sizeAdjuster;
        crhPosition = new Rect((Screen.width - csTextW) / 2,
           (Screen.height - csTextH) / 2, csTextW, csTextH); //determines width/height of our crosshair GUI texture
    }

    void CamLogic()
    {
        //Toggles cam location between left and right over the shoulder.
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (rShoulder == true)
            {
                Debug.Log("Camera is over your left shoulder");
                transform.localPosition = new Vector3(-1, 0, -4);
                rShoulder = false;
            }
            else
            {
                Debug.Log("Camera is over your right shoulder");
                transform.localPosition = new Vector3(1, 0, -4);
                rShoulder = true;
            }
        }
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
}


