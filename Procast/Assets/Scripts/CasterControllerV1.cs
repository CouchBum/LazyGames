using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CasterControllerV1 : MonoBehaviour
{

    protected enum CasterState
    {
        Idle = 0,
        CrouchingIdle = 1,
        CrouchWalking = 2,
        Walking = 3,
        Sprinting = 4,
        Falling = 5,
        Attacking = 6,
        Dead = 7
    }

    #region Variables
    CasterState currentState;
    CharacterController myCaster;
    public GameObject thisCaster;
    public GameObject deadCaster;
    public Camera casterCam;
    public GameObject casterHead;

    //skills = should move to own script
    public GameObject skill1;
    public GameObject skill2;

    bool fireballReady;
    bool firewallReady;
    bool afterburnerReady;
    bool flareboostersReady;

    //Game
    protected float team;
    protected Vector3 currentPosition;

    //States
    bool idle;
    bool walking;
    bool sprinting;
    bool turning;
    bool crouchingIdle;
    bool crouchWalking;
    bool dead;

    //UI Elements
    public Text healthText;

    //Stats
    public int health;
    protected float moveSpeed = 30f;
    protected float sprintSpeed = 5.5f;
    protected float rotSpeed = 700.0f;
    protected float detectRange;
    Quaternion targetRotation;

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
    //protected float etc.....

    public bool crouchToggle;
    public bool sprintToggle;


    public float gravity = 20.0F;
    protected Vector3 moveDirection;
    #endregion

    void Awake()
    {
        myCaster = GetComponent<CharacterController>();
        currentState = CasterState.Idle;
        targetRotation = transform.rotation;
        moveDirection = Vector3.zero;
        health = 100;
        //fireball = Resources.Load("Fireball") as GameObject;
        //firewall = Resources.Load("Firewall") as GameObject;
        /*
        crouchToggle = false;
        sprintToggle = false;
        fireballReady = true;
        firewallReady = true;
        afterburnerReady = true;
        flareboostersReady = true;
        */
    }

    void Update()
    {
        //RayCasting();
        HealthManager();
        InputHandler();
        StateHandler();
    }

    
    void HealthManager()
    {
        if (health <= 0)
        {
            currentState = CasterState.Dead;
        }
        
        if (Input.GetKeyDown(KeyCode.K)) //hit by attack
        {
            health -= 50; //attack damage
            //SetHealthText();
        }
    }
    
    /*
    void SetHealthText()
    {

        healthText.text = "Health: " + health.ToString();
    }
    */

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

    #region Skills
    void Attack1()
    {
        GameObject myFireball = Instantiate(skill1) as GameObject;
        myFireball.transform.position = casterCam.transform.position + casterCam.transform.forward * 10f;
        Rigidbody rb = myFireball.GetComponent<Rigidbody>();
        rb.velocity = casterCam.transform.forward * 50f;
    }

    void Attack2()
    {
        GameObject myFirewall = Instantiate(skill2) as GameObject;
        myFirewall.transform.position = myCaster.transform.position + casterCam.transform.forward * 10f;
        myFirewall.transform.rotation = myCaster.transform.rotation;
        Rigidbody rb = myFirewall.GetComponent<Rigidbody>();
    }

    void MoveSkill()
    {
        float thrustSpeed = 350f;
        moveDirection = new Vector3(0, .5f, thrustSpeed);
        moveDirection = transform.TransformDirection(moveDirection);
        myCaster.Move(moveDirection * Time.deltaTime);
    }

    void JumpSkill()
    {
        float ascendSpeed = 15f;
        moveDirection = new Vector3(0, ascendSpeed, 10f);
        moveDirection = transform.TransformDirection(moveDirection);
        myCaster.Move(moveDirection * Time.deltaTime);
    }
    #endregion

    void InputHandler()
    {
        //Axis Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");

        //turning with mouse
        targetRotation *= Quaternion.AngleAxis(rotSpeed * mouseX * Time.deltaTime, Vector3.up);
        myCaster.transform.rotation = targetRotation;

        if (currentState != CasterState.Dead)
        {
            if (myCaster.isGrounded)
            {
                //To Idle
                if (v == 0 && h == 0)
                {
                    currentState = CasterState.Idle;
                    sprintToggle = false;
                    moveDirection = Vector3.zero;
                }

                //To Walk or Sprint
                if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
                {
                    moveDirection = new Vector3(h, 0, v);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= moveSpeed;
                    currentState = CasterState.Walking;
                }
            }
            else
            {
                //gravity
                moveDirection.y -= gravity * Time.deltaTime;
            }
            myCaster.Move(moveDirection * Time.deltaTime);


            //FireballSkill (Mouse1)
            if (Input.GetButtonDown("Attack1"))
            {
                Attack1();
                //Invoke("Attack1", .5f);
            }

            //FireWall (Mouse2)
            if (Input.GetButtonDown("Attack2"))
            {
                Attack2();
                //Invoke("Attack2", 2f);
            }

            //Afterburner (F)
            if (Input.GetButtonDown("MoveSkill"))
            {
                MoveSkill();
            }

            //FlareBoosters (Space)
            if (Input.GetButtonDown("JumpSkill"))
            {
                JumpSkill();
            }
        }
    }

    void StateHandler()
    {
        if (currentState == CasterState.Dead)
        {
            KillSelf();
            //Invoke("Respawn", 6f);
        }
    }

    void KillSelf()
    {
        // Instantiate the wreck game object at the same position we are at
        GameObject wreckClone = (GameObject)Instantiate(deadCaster, transform.position, transform.rotation);
        // Kill ourselves
        Destroy(thisCaster);
    }

    void Respawn()
    {
        currentState = CasterState.Idle;
        GameObject respawnCaster = Instantiate(thisCaster) as GameObject;
        respawnCaster.transform.position = new Vector3(0, 150, 0);
    }
}