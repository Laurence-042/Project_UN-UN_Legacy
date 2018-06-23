using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Animator anim;

    public float reload=3;
    private float timer = 0;

    public float jumpSpeed = 15f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10f;
    public float minFall = -1.5f;

    private float _vertSpeed;

    private void Start()
    {
        _vertSpeed = minFall;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            anim.SetBool("Move", true);
            movement.z = z;
        }
        else
        {
            anim.SetBool("Move", false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
            anim.SetBool("Shoot", true);
        }
        else
        {
            anim.SetBool("Shoot", false);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Jump();
            anim.SetBool("Jump", true);
        }
        


        Ray ray = new Ray(this.transform.position, Vector3.down);




    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    void Fire()
    {
        if (timer <= 0)
        {
            timer = reload;
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            // Add velocity to the bullet
            // bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 600*Time.deltaTime;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 20.0f * Time.deltaTime);
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
    }

    void Jump()
    {

        transform.Translate(Vector3.up * 0.5f);

    }


}