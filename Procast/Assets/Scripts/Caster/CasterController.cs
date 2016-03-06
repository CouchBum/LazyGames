using UnityEngine;
using UnityEngine.UI;
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

    //UI Elements
    public Text healthText;

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

    public bool crouchToggle;
    public bool sprintToggle;


    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    void Awake()
    {
        myCaster = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentState = CasterState.Idle;
        targetRotation = transform.rotation;
        fireball = Resources.Load("Fireball") as GameObject;
        firewall = Resources.Load("Firewall") as GameObject;
        crouchToggle = false;
        sprintToggle = false;
        fireballReady = true;
        firewallReady = true;
        afterburnerReady = true;
        flareboostersReady = true;
        health = 100;
    }

    void Start()
    {
        //SetHealthText();
    }

    void Update()
    {
        RayCasting();
        HealthManager();       
        SkillHandler();
        CurrentStateCalculator();
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
            SetHealthText();
        }
    }

    void SetHealthText()
    {

        healthText.text = "Health: " + health.ToString();
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
            anim.SetTrigger("fireball");
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

    void CurrentStateCalculator()
    {
        //Axis Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        anim.SetFloat("vInput", v);
        anim.SetFloat("hInput", h);
        
        //gravity
        moveDirection.y -= gravity * Time.deltaTime;
        myCaster.Move(moveDirection * Time.deltaTime);

        //turning with mouse
        targetRotation *= Quaternion.AngleAxis(rotSpeed * mouseX * Time.deltaTime, Vector3.up);
        myCaster.transform.rotation = targetRotation;

        if (myCaster.isGrounded)
        {
            if (Input.GetButtonDown("Crouch"))
                CrouchToggle();

            //To Idle & Crouch Idle
            if (v == 0 && h == 0 && crouchToggle == false)
            {
                currentState = CasterState.Idle;
                sprintToggle = false;
                moveDirection = Vector3.zero;
            }
            else if (v == 0 && h == 0 && crouchToggle == true)
            {
                currentState = CasterState.CrouchingIdle;
                sprintToggle = false;
                moveDirection = Vector3.zero;
            }

                //To Walk & Crouch Walk & Sprint
                if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
            {
                if (Input.GetButtonDown("Sprint"))
                    SprintToggle();

                if (crouchToggle == false)
                {
                    moveDirection = new Vector3(h, 0, v);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= moveSpeed;
                    currentState = CasterState.Walking;
                }
                else if (crouchToggle == true)
                {
                    moveDirection = new Vector3(h, 0, v);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= moveSpeed/2;
                    currentState = CasterState.CrouchWalking;
                }
                if (sprintToggle == true)
                {
                    crouchToggle = false;
                    moveDirection = new Vector3(h, 0, v);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= sprintSpeed;
                    currentState = CasterState.Sprinting;
                }
            }
        }
        AnimationManager();
    }

    void CrouchToggle()
    {
        if (crouchToggle == true)
            crouchToggle = false;
        else
            crouchToggle = true;

    }

    void SprintToggle()
    {
        if (sprintToggle == true)
            sprintToggle = false;
        else
            sprintToggle = true;
    }

    void AnimationManager()
    {
        switch (currentState)
        {
            case CasterState.Idle:
                anim.SetBool("isIdle", true);
                anim.SetBool("isCrouchingIdle", false);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isSprinting", false);
                break;

            case CasterState.Walking:
                anim.SetBool("isWalking", true);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrouchingIdle", false);
                anim.SetBool("isSprinting", false);
                break;

            case CasterState.CrouchingIdle:
                anim.SetBool("isCrouchingIdle", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isSprinting", false);
                break;

            case CasterState.CrouchWalking:
                anim.SetBool("isCrouchWalking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrouchingIdle", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isSprinting", false);
                break;

            case CasterState.Sprinting:
                anim.SetBool("isSprinting", true);
                anim.SetBool("isWalking", false);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrouchingIdle", false);
                break;

            case CasterState.Falling:
                anim.SetBool("isFalling", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrouchingIdle", false);
                anim.SetBool("isSprinting", false);
                break;

            case CasterState.Dead:
                anim.SetBool("isDead", true);
                anim.SetBool("isWalking", false);
                anim.SetBool("isCrouchWalking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isSprinting", false);
                anim.SetBool("isCrouchingIdle", false);
                break;
        }
    }
}