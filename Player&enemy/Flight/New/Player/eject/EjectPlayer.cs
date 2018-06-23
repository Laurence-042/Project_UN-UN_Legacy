using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EjectPlayer : MonoBehaviour
{
    public GameObject player;
    [SerializeField] GameObject destroyedPlayerPlane;
    [SerializeField] GameObject destroyParticalesPre;
    [SerializeField] GameObject artParticalesPre;
    [SerializeField] float artDamage = 90f;
    [SerializeField] float destroyTime;
    [SerializeField] float destroyParticaleTime;
    [SerializeField] GameObject playerPre;

    private GameObject destroyParticales;

    private bool notGood = false;//渗透时被干掉了

    private float timer = 0;
    // Use this for initialization
    void Start()
    {
        if (player == null||player.tag!="Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (artParticalesPre == null)
        {
            artParticalesPre = Resources.Load<GameObject>("Prefabs/Effect/ArtParticle");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            if (players[0].name == "Player(Clone)" && players[1].name == "Player(Clone)")
            {
                Destroy(players[1]);
            }
        }
        if (player == null || player.tag != "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        /*
        if (player == null||player.tag!="Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null&&!notGood)
            {
                Debug.Log("WTF");
                GetComponent<Center>().stealSuccessfully = false;
                notGood = true;
            }
        }
        else
        {
            notGood = false;
        }
        */
        timer -= Time.deltaTime;
        if (CrossPlatformInputManager.GetButtonDown("Art"))
        {
            if (GameObject.FindGameObjectWithTag("temp")||timer>0)
            {
                
            }
            else
            {
                timer = 6;
                if (GameObject.FindGameObjectWithTag("Player"))
                {
                    StartCoroutine(ReadyForArt());
                }
                else
                {
                    StartCoroutine(GetComponent<Center>().ResetPlayer());
                }
            }
        }
    }

    IEnumerator ReadyForArt()
    {
        yield return new WaitForEndOfFrame();
        if (player.GetComponent<NewHp>())
        {
            player.GetComponent<NewHp>().hp = 0;
            ActiveEject();
            StartCoroutine(Art());
        }
        else
        {
            Debug.Log("should reset");
            player.tag = "temp";
            StartCoroutine( GetComponent<Center>().ResetPlayer());
            Destroy(player,6f);
            //StartCoroutine(GetComponent<Center>().ResetPlayer());
        }
    }

    public void ActiveEject()
    {
        destroyedPlayerPlane = player;
        player.tag = "temp";
        destroyParticales = Instantiate(destroyParticalesPre, destroyedPlayerPlane.transform.position, Quaternion.Euler(0, 0, 0));
        player = Instantiate(playerPre, destroyedPlayerPlane.transform.position, Quaternion.Euler(0, 0, 0), GameObject.FindGameObjectWithTag("KeepPlayer").transform);
        //player.transform.parent = GameObject.FindGameObjectWithTag("KeepPlayer").transform;
        StartCoroutine(Ejecting());

    }

    IEnumerator Ejecting()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("eject");
        destroyedPlayerPlane.GetComponent<NewHp>().burst = true;

        yield return new WaitForSeconds(destroyTime);
        if (destroyedPlayerPlane.GetComponentInChildren<Animator>())
        {
            GameObject temp_playerModel = destroyedPlayerPlane.GetComponentInChildren<Animator>().gameObject;
            Destroy(temp_playerModel);
        }

        destroyedPlayerPlane.GetComponent<NewHp>().finalBurst = true;

        yield return new WaitForSeconds(destroyParticaleTime);
        Destroy(destroyParticales);

        yield return new WaitForSeconds(destroyTime);
        Destroy(destroyedPlayerPlane);
        foreach(GameObject temp in GameObject.FindGameObjectsWithTag("temp"))
        {
            Destroy(temp);
        }
    }

    IEnumerator Art()
    {
        yield return new WaitForSeconds(destroyTime);
        GameObject artParticales = Instantiate(artParticalesPre, destroyedPlayerPlane.transform.position, Quaternion.Euler(0, 0, 0));

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, destroyedPlayerPlane.transform.position) < artParticales.GetComponent<ParticleSystem>().main.startSpeedMultiplier * artParticales.GetComponent<ParticleSystem>().main.startLifetimeMultiplier)
            {
                enemy.GetComponent<NormalArmor>().TakeDamage(artDamage, enemy.transform.position);
            }
        }
        yield return new WaitForSeconds(destroyParticaleTime);
        Destroy(artParticales);
    }

}
