using UnityEngine;
using System.Collections;

public class CasterController : MonoBehaviour {

    CharacterController myCaster;
    private Animator anim;
    public Camera casterCam;
    public GameObject casterHead;
    GameObject fireball;
    GameObject firewall;

    bool fireballReady;
    bool firewallReady;
    bool afterburnerReady;
    bool flareboostersReady;

    //Game
    protected float team;
    protected Vector3 currentPosition;

    //State


    //Stats
    protected float health;
    protected float mana;
    protected float moveSpeed = 5.0f;
    protected float sprintSpeed = 2.0f;
    protected float rotSpeed = 1.0f;
    protected float vertLookSpeed = 1.0f;
    protected float detectRange;

    //protected Affinity affinity; 

    //Skills
    protected Skill[] skills;

    //Transformation
    protected bool canTransform;
    protected bool isTransformed;

    //inventory
    protected Item[] items;

    //Passive Skills (Buffs/Debuffs)
    //protected Passive[] passives;

    //Active Skills
    //protected Active[] actives;

    //User Account Totals
    protected float damageDealt;
    protected float damageTaken;
    //protected float et.....


    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
        myCaster = GetComponent<CharacterController>();
        fireball = Resources.Load("Fireball") as GameObject;
        firewall = Resources.Load("Firewall") as GameObject;
        fireballReady = true;
        firewallReady = true;
        afterburnerReady = true;
        flareboostersReady = true;
    }

    void Update()
    {
        RayCasting();
        MovementHandler();
        SkillHandler();
    }

    void RayCasting()
    {
        Vector3 forward = casterCam.transform.TransformDirection(Vector3.forward) * 1000f;
        //Debug.DrawRay(casterHead.transform.position, forward, Color.blue);

        //RayCast from Camera
        RaycastHit hit;
        Vector3 hitPoint;
        Ray aimingRay = new Ray(casterCam.transform.position, forward);

        if (Physics.Raycast(aimingRay, out hit))
        {
            hitPoint = hit.point;
            Debug.Log(hit.collider.tag);
            Debug.DrawLine(casterCam.transform.position, hitPoint, Color.green);
            Debug.DrawLine(casterHead.transform.position, hitPoint, Color.red);
        }
        else
        {
            //hitPoint = forward;
        }
    }

    void SkillHandler()
    {
        //FireballSkill (Mouse1)
        if (Input.GetMouseButtonDown(0) && fireballReady == true)
        {
            anim.Play("Standing_Walk_Forward", -1, 0f);
            GameObject myFireball = Instantiate(fireball) as GameObject;
            myFireball.transform.position = casterCam.transform.position + casterCam.transform.forward * 10f;
            //myFireball.transform.position = casterHead.transform.position + casterHead.transform.forward * myFireball.transform.localScale.y;
            Rigidbody rb = myFireball.GetComponent<Rigidbody>();
            rb.velocity = casterCam.transform.forward * 50f;
            //rb.velocity = casterHead.transform.forward * 50f;

            //fireballReady = false;
            Debug.Log("Fireball On Cooldown");

            if (fireballReady == false)
            {

            }
        }

        //FireWall (Mouse2)
        if (Input.GetMouseButtonDown(1))
        {
            GameObject myFirewall = Instantiate(firewall) as GameObject;
            myFirewall.transform.position = myCaster.transform.position + casterCam.transform.forward * 10f;
            myFirewall.transform.rotation = myCaster.transform.rotation;
            Rigidbody rb = myFirewall.GetComponent<Rigidbody>();
        }

        //FlareBoosters (Space)
        float ascendSpeed = 10f;
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection = new Vector3(0, ascendSpeed, 0);
            moveDirection = transform.TransformDirection(moveDirection);
        }

        //Afterburner (F)
        if (Input.GetKeyDown(KeyCode.F))
        {

        }
    }

    void MovementHandler()
    {
        //align with camera
        //code here...

        if (myCaster.isGrounded)
        {
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");
            anim.SetFloat("vInput", vAxis);
            anim.SetFloat("hInput", hAxis);

            //Idle
            if (hAxis == 0 && vAxis == 0)
            {
                anim.SetBool("Idle", true);
            }
            else
                anim.SetBool("Idle", false);

            //forward, backward, strafing movement
            moveDirection = new Vector3(hAxis, 0, vAxis);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            //Sprinting
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= sprintSpeed;
                anim.SetBool("isSprinting", true);
            }
            else
                anim.SetBool("isSprinting", false);
        }
        //turning with mouse
        float h = rotSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);



        //gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //needs this to work
        myCaster.Move(moveDirection * Time.deltaTime);
    }
}