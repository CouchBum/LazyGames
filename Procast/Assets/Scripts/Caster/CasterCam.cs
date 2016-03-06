using UnityEngine;
using System.Collections;

public class CasterCam : MonoBehaviour
{
    public GameObject casterHead;
    public Texture2D crosshairTex;
    private Rect crhPosition;

    //Camera Smoothing and moving forward/back
    public Transform camTarget;
    public float smoothTime = 0.3F;
    private Vector3 camVel = Vector3.zero;

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
        //CrosshairPosition();
        StopClipping();
    }

    void LateUpdate()
    {
        CamLogic();
    }

    void OnGUI()
    {
        //GUI.DrawTexture(crhPosition, crosshairTex);
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
                Vector3 targetPosition = camTarget.TransformPoint(new Vector3(-1, 0, -4));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camVel, smoothTime);
                rShoulder = false;
            }
            else
            {
                Debug.Log("Camera is over your right shoulder");
                Vector3 targetPosition = camTarget.TransformPoint(new Vector3(1, 0, -4));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camVel, smoothTime); rShoulder = true;
            }
        }
    }

    void StopClipping()
    {
        //RayDirction (towards caster Head)
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000f;
        Vector3 targetDir = casterHead.transform.position - transform.position;

        RaycastHit hit;
        Ray aimingRay = new Ray(transform.position, targetDir);
        Debug.DrawRay(transform.position, targetDir, Color.yellow);

        if (Physics.Raycast(aimingRay, out hit))
        {
            if (hit.collider.tag != "Player")
            {
                Vector3 targetPosition = camTarget.TransformPoint(new Vector3(1, 1, 0));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camVel, smoothTime);
                Debug.Log("Not Hitting the Player");
            }
            else
            {
                /*
                Vector3 targetPosition = camTarget.TransformPoint(new Vector3(1, 1, -4));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camVel, smoothTime);
                Debug.Log("Hitting Player");
                */
            }
        }
    }
}


