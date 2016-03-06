using UnityEngine;
using System.Collections;

public class CasterCam : MonoBehaviour
{
    public GameObject casterHead;
    public Texture2D crosshairTex;
    private Rect crhPosition;

    private float zPosition;
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
    }

    void LateUpdate()
    {
        StopClipping();
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

    void StopClipping()
    {
        //RayDirction (towards caster Head)
        Vector3 targetDir = casterHead.transform.position - transform.position;

        RaycastHit hit;
        Ray aimingRay = new Ray(transform.position, targetDir);
        Debug.DrawRay(transform.position, targetDir, Color.yellow);

        if (Physics.Raycast(aimingRay, out hit))
        {
            if (hit.collider.tag != "Player")
            {
                //this works so you're doing something right
                //transform.localPosition = new Vector3(1, 1, 1);
                Debug.Log("Not Hitting the Player");
            }
            else
                Debug.Log("Hitting Player");
            //--move camera up in transform.position.forward.
        }
    }
}


