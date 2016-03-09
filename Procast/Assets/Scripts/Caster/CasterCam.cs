using UnityEngine;
using UnityEngine.Networking;

public class CasterCam : NetworkBehaviour
{
    public GameObject casterHead;
    //public Texture2D crosshairTex;
    //private Rect crhPosition;

    //Camera Smoothing and moving forward/back
    //public Transform camTarget;
    private Vector3 offset;
    //private Vector3 zoomOffset = new Vector3(10, 1, -1);
    //public float smoothTime = 0.3F;
    //private Vector3 camVel = Vector3.zero;

    //private float csTextH;
    //private float csTextW;
    //bool rShoulder;

    void Start()
    {
        offset = transform.position - casterHead.transform.position;
        //rShoulder = true;
    }

    /*
    void Update()
    {
        //CrosshairPosition();
        //StopWallClipping();
    }

    void LateUpdate()
    {
        //transform.position = casterHead.transform.position + offset;
        //CamLogic();
    }

    void OnGUI()
    {
        GUI.DrawTexture(crhPosition, crosshairTex);
    }

    void CrosshairPosition()
    {
        csTextH = crosshairTex.height;
        csTextW = crosshairTex.width;
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
    

    void StopWallClipping()
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
                transform.position = Vector3.SmoothDamp(transform.position, zoomOffset, ref camVel, smoothTime);
                Debug.Log("Not Hitting the Player");
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, offset, ref camVel, smoothTime);
                /*
                Vector3 targetPosition = camTarget.TransformPoint(new Vector3(1, 1, -4));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camVel, smoothTime);
                Debug.Log("Hitting Player");
            }
        }
    }
*/
}

//Vector3 targetPosition = camTarget.TransformPoint(new Vector3(1, 1, 0));


