using UnityEngine;
using System.Collections;

public class CasterController : MonoBehaviour {

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
    public GameObject thisCaster;
    CasterState currentState;
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

    //States
    bool idle;
    bool walking;
    bool sprinting;
    bool turning;
    bool crouchingIdle;
    bool crouchWalking;
    bool dead;

    //Stats
    public int health;
    protected float mana;
    protected float moveSpeed = 3f;
    protected float sprintSpeed = 5.5f;
    protected float rotSpeed = 700.0f;
    protected float vertLookSpeed = 1.0f;
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
    //protected float et.....


    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    void Start()
    {
        myCaster = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        health = 100;
        targetRotation = transform.rotation;
        fireball = Resources.Load("Fireball") as GameObject;
        firewall = Resources.Load("Firewall") as GameObject;
        fireballReady = true;

        //Rename Animator Bools to make life easier
        idle = anim.GetBool("isIdle");
        walking = anim.GetBool("isWalking");
        sprinting = anim.GetBool("isSprinting");
        turning = anim.GetBool("isTurning");
        crouchingIdle = anim.GetBool("isCrouchingIdle");
        crouchWalking = anim.GetBool("isCrouchWalking");
        dead = anim.GetBool("isDead");
        
        
        currentState = CasterState.Idle;
        /*
        firewallReady = true;
        afterburnerReady = true;
        flareboostersReady = true;
        */
    }

    void Update()
    {
        RayCasting();
        HealthManager();       
        //MovementHandler();
        SkillHandler();
        CharStateManager();
        AnimationManager();
    }


    void HealthManager()
    {
        //anim.SetInteger("health", health);
        if (health > 0)
                anim.SetBool("isDead", false);
        else
        {
            anim.SetBool("isDead", true);
            Destroy(thisCaster, 5f);
        }
        
        if (Input.GetKeyDown(KeyCode.K)) //hit by attack
        {
            health -= 50; //attack damage
        }
        
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
            anim.Play("FireBall");
            //anim.SetBool("isFireball", true);
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
        anim.SetFloat("speed", moveSpeed);
        //align with camera
        //code here...

        if (myCaster.isGrounded)
        {
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");
            anim.SetFloat("vInput", vAxis);
            anim.SetFloat("hInput", hAxis);

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

        //gravity
        //moveDirection.y -= gravity * Time.deltaTime;
        //needs this to work
        //myCaster.Move(moveDirection * Time.deltaTime);
    }

    void CharStateManager()
    {
        //Axis Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        anim.SetFloat("vInput", v);
        anim.SetFloat("hInput", h);
        
        //gravity
        moveDirection.y -= gravity * Time.deltaTime;
        myCaster.Move(moveDirection * Time.deltaTime);

        //turning with mouse
        targetRotation *= Quaternion.AngleAxis(rotSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;

        //State Logic
        #region Movement
        if (myCaster.isGrounded)
        {
            //to Idle
            if (h == 0 && v == 0)
            {
                currentState = CasterState.Idle;
                moveDirection = Vector3.zero;

                //if Idle
                if (currentState == CasterState.Idle) 
                {
                    //to Crouch Idle
                    if (Input.GetKey(KeyCode.C))
                    {
                        currentState = CasterState.CrouchingIdle;
                    }
                }
            }
            //to Walk
            else if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
            {
                moveDirection = new Vector3(h, 0, v);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= moveSpeed;
                currentState = CasterState.Walking;

                //If Walking
                if (currentState == CasterState.Walking)
                {
                    //to Sprint
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        moveDirection = new Vector3(h, 0, v);
                        moveDirection = transform.TransformDirection(moveDirection);
                        moveDirection *= sprintSpeed;
                        currentState = CasterState.Sprinting;
                    }
                    else if (Input.GetKey(KeyCode.C))
                    {
                        currentState = CasterState.CrouchWalking;
                    }
                }
            }
        }
        else
        {
            currentState = CasterState.Falling;
        }
        #endregion
    }  

    void AnimationManager()
    {
        if (currentState == CasterState.Idle)
        {
            //anim.Play("Idle");
            Debug.Log("Idle");
            anim.SetBool("isIdle", true);
            anim.SetBool("isCrouchingIdle", false);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isSprinting", false);
        }
        if (currentState == CasterState.CrouchingIdle)
        {
            //anim.Play("CrouchingIdle");
            Debug.Log("CrouchingIdle");
            anim.SetBool("isCrouchingIdle", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isSprinting", false);
        }
        if (currentState == CasterState.CrouchWalking)
        {
            //anim.Play("CrouchWalking");
            Debug.Log("CrouchWalking");
            anim.SetBool("isCrouchWalking", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrouchingIdle", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isSprinting", false);
        }
        if (currentState == CasterState.Walking)
        {
            //anim.Play("Walking");
            Debug.Log("Walking");
            anim.SetBool("isWalking", true);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrouchingIdle", false);
            anim.SetBool("isSprinting", false);
        }
        if (currentState == CasterState.Sprinting)
        {
            //anim.Play("Sprinting");
            Debug.Log("Sprinting");
            anim.SetBool("isSprinting", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrouchingIdle", false);
        }
        if(currentState == CasterState.Falling)
        {
            //anim.Play("Falling");
            Debug.Log("Falling");
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrouchingIdle", false);
            anim.SetBool("isSprinting", false);

        }
        if (currentState == CasterState.Attacking)
        {
            //anim.Play("Attacking");
            Debug.Log("Attacking");
        }
        if (currentState == CasterState.Dead)
        {
            //anim.Play("Dead");
            Debug.Log("You're Dead");
            anim.SetBool("isDead", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isSprinting", false);
            anim.SetBool("isCrouchingIdle", false);
        }
    }
}