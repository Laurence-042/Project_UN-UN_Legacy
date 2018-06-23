using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThiefFight : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float range = 10f;                      // The distance the gun can fire.
    public float reAttckTime = 5f;
    public float chargeTime=3f;

    
    [SerializeField] Transform gunEnd;
    [SerializeField] AudioSource chargeAuSource;
    [SerializeField] AudioSource shootAuSource;
    [SerializeField] ParticleSystem chargeEffect;
    [SerializeField] ParticleSystem shootEffect;
    [SerializeField] GameObject bulletPre;

    [SerializeField] bool charging = false;
    private float timer = 0f;

    private void Start()
    {
        bulletPre = Resources.Load<GameObject>("Prefabs/FlyBullet");

    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fight"))
        {
            if (!charging)
            {
                charging = true;
                timer = 0f;
            }
        }
        if (CrossPlatformInputManager.GetButton("Fight")&&charging)
        {
            timer += Time.deltaTime;
            if (!chargeAuSource.isPlaying) {
                chargeAuSource.Play(); 
            }
            if (!chargeEffect.isPlaying)
            {
                chargeEffect.Play();
            }

            if (timer >= chargeTime)
            {
                chargeAuSource.Stop();

                shootAuSource.Play();

                shootEffect.Play();
                Instantiate(bulletPre, gunEnd.position, gunEnd.rotation);
                timer = 0f;
                charging = false;
            }
        }
        else
        {
            charging = false;
            
            chargeAuSource.Stop();
            
            timer = 0;
        }
        
    }


    /*
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.

    public float range = 10f;                      // The distance the gun can fire.
    public float reAttckTime = 5f;

    public float timeBetweenComboSlash;                     //连击许可时段
    public float comboSlashTimer;                           //连击计时
    public float slash0Time = 0.5f;
    public float slash1Time = 1.5f;
    public bool combo = false;
    public bool heavy = false;

    public float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    //ParticleSystem gunParticles;                    // Reference to the particle system.
    //LineRenderer gunLine;                           // Reference to the line renderer.
    //[SerializeField] AudioSource gunAudio;                           // Reference to the audio source.
    [SerializeField] Light gunLight;                                 // Reference to the light component.
    //public Light faceLight;                             // Duh
    float effectsDisplayTime = 0.5f;                // The proportion of the timeBetweenBullets that the effects will display for.

    [SerializeField] Animator anim;
    [SerializeField] AudioClip softAttackClip;
    [SerializeField] AudioClip hardAttackClip;
    [SerializeField] AudioSource auSource;
    // Use this for initialization
    void Start()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");

        anim = GetComponent<Animator>();
        auSource = GetComponent<AudioSource>();
        gunLight.enabled = false;
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;


        // If there is input on the shoot direction stick and it's time to fire...
        bool fight = CrossPlatformInputManager.GetButtonDown("Fight");

        if (fight && (timer >= reAttckTime || combo))
        {
            if (combo)
            {
                heavy = true;
                combo = false;
                comboSlashTimer = 0f;
            }
            else
            {
                combo = true;
                timer = 0f;
                StartCoroutine(Slash0());
            }
        }
        if (combo)//计时,到时间后重置计时器并结束combo
        {
            comboSlashTimer += Time.deltaTime;
            if (comboSlashTimer > timeBetweenComboSlash)
            {
                combo = false;
                heavy = false;
                comboSlashTimer = 0f;
            }
        }

    }

    IEnumerator Slash0()
    {

        anim.SetBool("Slash", true);

        yield return new WaitForSeconds(slash0Time);
        // Enable the lights.
        gunLight.enabled = true;

        //gunAudio.Play();

        Debug.Log("Slash0");

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            Debug.Log(shootHit);
            auSource.clip = softAttackClip;
            auSource.Play();
            // Try and find an EnemyHealth script on the gameobject hit.
            MechHealth enemyHealth = shootHit.collider.GetComponent<MechHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
        }
        StartCoroutine(Slash1());
        if (!heavy)
        {
            timer += slash1Time+0.2f;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Slash", false);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            auSource.Play();
        }
        yield return new WaitForSeconds(effectsDisplayTime);
        gunLight.enabled = false;
    }

    IEnumerator Slash1()
    {
        if (heavy)
        {
            

            heavy = false;
            anim.SetBool("HeavySlash", true);

            yield return new WaitForSeconds(slash1Time);
            // Enable the lights.
            gunLight.enabled = true;

            //gunAudio.Play();

            Debug.Log("Slash1");

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                auSource.clip = hardAttackClip;
                auSource.Play();
                // Try and find an EnemyHealth script on the gameobject hit.
                MechHealth enemyHealth = shootHit.collider.GetComponent<MechHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(2*damagePerShot, shootHit.point);
                }
            }
            anim.SetBool("HeavySlash", false);
            anim.SetBool("Slash", false);
            yield return new WaitForSeconds(effectsDisplayTime);
            gunLight.enabled = false;
        }
        else
        {
            yield return null;
        }
    }
    */
}
