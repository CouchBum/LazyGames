using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CasterControllerV1 : NetworkBehaviour
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
    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIinstance;


    //skills = should move to own script
    public GameObject skill1Prefab;
    public GameObject skill2Prefab;

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
    private int maxHealth = 100;
    [SyncVar]
    public int currentHealth;

    public float moveSpeed;
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
        playerUIinstance = Instantiate(playerUIPrefab);
        currentHealth = maxHealth;
        moveSpeed = 10f;
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
        if(isLocalPlayer)
        {
            //RayCasting();
            //HealthManager();
            InputHandler();
            CmdStateHandler();
        }
    }
        
    /*
    void SetHealthText()
    {

        healthText.text = "Health: " + health.ToString();
    }
    */

    public void TakeDamage(int damageTaken, Vector3 damageDirection)
    {
        //if hit by an attack, send this to health/screen display
        if (!isServer)
            return;

        currentHealth -= damageTaken;
        Debug.Log("You took damage");

        if (currentHealth <= 0)
        {
            currentState = CasterState.Dead;
        }
    }
 
    private RaycastHit RayCasting()
    {
        Vector3 forward = casterCam.transform.TransformDirection(Vector3.forward) * 1000f;
        Debug.DrawRay(casterCam.transform.position, forward, Color.blue);

        //RayCast from Camera
        RaycastHit hit;
        Ray aimingRay = new Ray(casterCam.transform.position, forward);

        if (Physics.Raycast(aimingRay, out hit))
        {
            Debug.Log("You Hit " + hit.collider.tag);
            //Debug.DrawLine(casterCam.transform.position, hit.point, Color.green);
            //Debug.DrawLine(casterHead.transform.position, hit.point, Color.red);
            return hit;
        }
        else
        {
            //hitPoint = forward;
            return hit;
        }
    }

    #region Skills
    [Command]
    void CmdAttack1()
    {
        Vector3 attack1Position = casterHead.transform.position + casterHead.transform.forward * 3f;
        Quaternion attackRotation = casterCam.transform.rotation;
        GameObject myFireball = Instantiate(skill1Prefab, attack1Position, attackRotation) as GameObject;
        Rigidbody rb = myFireball.GetComponent<Rigidbody>();
        rb.velocity = myFireball.transform.forward * 50f;
        NetworkServer.Spawn(myFireball);
    }

    void Attack2()
    {
        GameObject myFirewall = Instantiate(skill2Prefab) as GameObject;
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
                    moveDirection = Vector3.zero;
                }

                //To Move
                if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
                {
                    if (Mathf.Abs(h) >= .75 && Mathf.Abs(v) >= .75)
                    {
                        if(h >= .75)
                            h = .75f;
                        if (h <= -.75)
                            h = -.75f;
                        if (v >= .75)
                            v = .75f;
                        if (v <= -.75)
                            v = -.75f;
                    }
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
                CmdAttack1();
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

    [Command]
    void CmdStateHandler()
    {
        if (currentState == CasterState.Dead)
        {
            KillSelf();
        }
    }

    void KillSelf()
    {
        if (!isServer)
            return;
        
        // Instantiate the wreck game object at the same position we are at
        GameObject wreckClone = (GameObject)Instantiate(deadCaster, transform.position, transform.rotation);
        // Kill ourselves
        Destroy(playerUIinstance);
        Destroy(thisCaster);
    }

    void Respawn()
    {
        currentState = CasterState.Idle;
        GameObject respawnCaster = Instantiate(thisCaster) as GameObject;
        respawnCaster.transform.position = new Vector3(0, 150, 0);
    }
}