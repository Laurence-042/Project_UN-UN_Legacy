using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;


public class ThiefMovement : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject[] JoySticks0;
    [SerializeField] Vector3[] JoySticksScale0;
    [SerializeField] GameObject[] JoySticks1;
    [SerializeField] Vector3[] JoySticksScale1;

    public float speed = 6f;            // The speed that the player will move at.
    public float verSpeed;

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    [SerializeField] Animator anim;                      // Reference to the animator component.
    CharacterController controller;
    [SerializeField] AudioSource walkAuSource;
    [SerializeField] AudioSource flyAuSource;

    public bool onGround = true;
    public float reJumpDelay = 0.5f;
    public float jumpSpeed = 15f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minfall = -1.5f;
    public float jumpTimer = 0;

    public bool crouch = false;
    [SerializeField] ParticleSystem crouchEffect;

    public bool turning = false;

    [SerializeField] bool flyMode = false;
    [SerializeField] bool flyToEnemy = false;
    [SerializeField] bool flyBack = false;
    [SerializeField] GameObject flyModeTrail;
    [SerializeField] Vector3 flyDirect = Vector3.zero;
    public float flySpeed = 6f;
    [SerializeField] float max_flySpeed = 10f;
    [SerializeField] float flyDropSpeed = 1f;
    [SerializeField] float flySpeedAdd = 1f;
    [SerializeField] Transform enemyPos;

    public float zoneZoom = 1f;

    void Awake()
    {
        // Set up references.
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        controller = GetComponent<CharacterController>();

        flyModeTrail.transform.localScale = Vector3.zero;
        verSpeed = minfall;

        //JoySticks0 = GameObject.FindGameObjectsWithTag("MoveButton");

        for (int i = 0; i < JoySticks0.Length; i++)
        {
            JoySticksScale0[i] = JoySticks0[i].transform.localScale;
        }
        for (int i = 0; i < JoySticks1.Length; i++)
        {
            JoySticksScale1[i] = JoySticks1[i].transform.localScale;
        }
    }


    void FixedUpdate()
    {
        SetFlyMode();
        if (!flyMode)
        {
            WalkMode();
            for (int i = 0; i < JoySticks0.Length; i++)
            {
                JoySticks0[i].transform.localScale = JoySticksScale0[i];
            }
            for (int i = 0; i < JoySticks1.Length; i++)
            {
                JoySticks1[i].transform.localScale = Vector3.zero;
            }
        }
        else
        {
            FlyMode();
            for (int i = 0; i < JoySticks0.Length; i++)
            {
                JoySticks0[i].transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < JoySticks1.Length; i++)
            {
                JoySticks1[i].transform.localScale = JoySticksScale0[i];
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && flyToEnemy)
        {
            flyToEnemy = false;
            flyBack = true;
            flyDirect = -flyDirect;
            flySpeed = 5f;
            other.GetComponent<MechHealth>().TakeDamage(100, other.transform.position);
        }
    }



    void FlyMode()
    {
        //点击&&不在防御模式&&点在UI上
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log(Input.GetTouch(0).fingerId + " " + EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) )
            {
                
                flyAuSource.Play();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        enemyPos = hit.transform;
                        flyToEnemy = true;
                    }
                    else
                    {
                        flyToEnemy = false;
                        flyBack = false;
                    }
                    Vector3 toPoint = hit.point;
                    toPoint.x = transform.position.x;

                    flyDirect = toPoint - transform.position;
                    flyDirect = Vector3.Normalize(flyDirect);

                    if (flyToEnemy)
                    {
                        flySpeed = max_flySpeed;
                        mainCamera.GetComponent<Follow>().lerp_t = 0.03f;
                        mainCamera.GetComponent<Follow>().m_offset.x = flySpeed * 1.5f + mainCamera.GetComponent<Follow>().offset.x*zoneZoom;
                    }
                    else
                    {
                        if (max_flySpeed > flySpeed)
                        {
                            flySpeed += flySpeedAdd;
                            mainCamera.GetComponent<Follow>().lerp_t = 0.1f;
                            mainCamera.GetComponent<Follow>().m_offset.x = flySpeed + mainCamera.GetComponent<Follow>().offset.x*zoneZoom;
                        }
                    }

                }
            }
        }

        if (controller.isGrounded && flyDirect.y <= 0)
        {
            flyMode = false;
            flyBack = false;
            flyToEnemy = false;
            flyModeTrail.transform.localScale = Vector3.zero;
            flyDirect = Vector3.zero;
            flySpeed = 3f;
            mainCamera.GetComponent<Follow>().acceptableAngle = 0.2f;
            mainCamera.GetComponent<Follow>().lerp_t = 3f;
            mainCamera.GetComponent<Follow>().m_offset = mainCamera.GetComponent<Follow>().offset*zoneZoom;
            anim.SetBool("InAir", false);
        }
        else
        {
            if (flyDirect != Vector3.zero)
            {
                mainCamera.GetComponent<Follow>().acceptableAngle = 0.05f;
                if (!flyToEnemy)
                {
                    //防止撞上天花板
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position+2f*Vector3.up, Vector3.up, out  hit,1.3f))
                    {
                        if (flyDirect.y > 0)
                        {
                            flyDirect.y = 0;
                        }
                        //Debug.Log(hit.collider.gameObject.name);
                    }
                    Vector3 flyMovement = new Vector3(0, flyDirect.y * 20 * Time.deltaTime, flyDirect.z * flySpeed * 5f * Time.deltaTime);
                    controller.Move(flyMovement);
                    flyDirect.y -= flyDropSpeed * Time.deltaTime;

                    Vector3 faceTo = transform.position + flyDirect;
                    if (Vector3.Angle(flyDirect, Vector3.down) < 45f)
                    {

                        if (flyDirect.z > 0)
                        {
                            faceTo = transform.position + new Vector3(0, -1, 2);
                        }
                        else
                        {
                            faceTo = transform.position + new Vector3(0, -1, -2);
                        }
                    }
                    else if (Vector3.Angle(flyDirect, Vector3.up) < 45f)
                    {
                        if (flyDirect.z > 0)
                        {
                            faceTo = transform.position + new Vector3(0, 1, 2);
                        }
                        else
                        {
                            faceTo = transform.position + new Vector3(0, 1, -2);
                        }
                    }
                    if (flyBack)
                    {
                        faceTo = -faceTo + 2 * transform.position;
                    }
                    controller.transform.LookAt(faceTo);
                    flySpeed -= Time.deltaTime;
                }
                else
                {
                    flyDirect = enemyPos.position - transform.position;
                    flyDirect = Vector3.Normalize(flyDirect);
                    Vector3 flyMovement = new Vector3(0, flyDirect.y * max_flySpeed * 5f * Time.deltaTime, flyDirect.z * max_flySpeed * 5f * Time.deltaTime);
                    controller.Move(flyMovement);
                    controller.transform.LookAt(transform.position + flyMovement);
                }
            }

        }

    }

    void WalkMode()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        onGround = controller.isGrounded;

        // Store the input axes.
        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (v != 0&&!crouch&&onGround)
        {
            if (!walkAuSource.isPlaying)
            {
                walkAuSource.Play();
            }
        }
        else
        {
            walkAuSource.Stop();
        }
        // Move the player around the scene.
        Move(h, v);
        // Turn the player to face the mouse cursor.

        //controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.LookRotation(transform.position + h * Vector3.forward), 90*Time.deltaTime);
        Quaternion new_rot = Quaternion.Euler(0, Mathf.Lerp(controller.transform.rotation.eulerAngles.y, h > 0 ? 0 : h < 0 ? 180 : controller.transform.rotation.eulerAngles.y, 10f * Time.deltaTime), 0);
        controller.transform.rotation = new_rot;
        //controller.transform.LookAt(transform.position + h * Vector3.forward);

        // Animate the player.
        Animating(h, v);

        if (onGround)
        {
            if (v > 0.7f && jumpTimer <= 0)
            {
                verSpeed = jumpSpeed;
                jumpTimer = reJumpDelay;
                anim.SetBool("Jump", true);
            }
            else
            {
                verSpeed = minfall;
                anim.SetBool("Jump", false);
            }
        }
        else
        {
            verSpeed += gravity * 5 * Time.deltaTime;
            if (verSpeed < terminalVelocity)
            {
                verSpeed = terminalVelocity;
            }
        }

        movement.y = verSpeed;
        //movement *= Time.deltaTime;

        controller.Move(movement);

        mainCamera.GetComponent<Follow>().acceptableAngle = 0.2f;
        mainCamera.GetComponent<Follow>().lerp_t = 3f;
        mainCamera.GetComponent<Follow>().m_offset = mainCamera.GetComponent<Follow>().offset * zoneZoom;
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        //movement.Set(h, 0f, v);
        movement.Set(v, 0f, h);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        movement.x = 0;
        // Move the player to it's current position plus the movement.

    }


    void Animating(float h, float v)
    {
        /*
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
        */
        if (!onGround)
        {
            anim.SetBool("InAir", true);
        }
        else
        {
            anim.SetBool("InAir", false);

        }
        if (h != 0 && v >= -0.7)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        if (v < -0.7f)
        {
            anim.SetBool("Crouch", true);
            movement.z = 0;
            if (!crouch)
            {
                crouchEffect.Clear();
                crouch = true;
            }
            
            if (!crouchEffect.isPlaying)
            {
                crouchEffect.Play();
            }
        }
        else
        {
            crouch = false;
            crouchEffect.Stop();
            anim.SetBool("Crouch", false);
        }
    }


    void SetFlyMode()
    {
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.GetTouch(0).phase == TouchPhase.Began)
            {//mobile end
                Debug.Log("mot ui");
                flyMode = true;
                Debug.Log("fly mode on");
                flyModeTrail.transform.localScale = new Vector3(1, 1, 1);
                anim.SetBool("InAir", true);

            }
        }

    }

}
