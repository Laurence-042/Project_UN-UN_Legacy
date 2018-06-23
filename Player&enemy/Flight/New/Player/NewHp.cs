using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHp : MonoBehaviour
{


    [SerializeField] Slider hpbar;
    [SerializeField] GameObject hitParticlePre;
    [SerializeField] ParticleSystem burningParticle;
    public float maxHp = 100.0f;
    public float hp = 100.0f;
    public float senDamage = 6.0f;
    [SerializeField] float hitEffectTime = 1f;
    [SerializeField] GameObject ausourcePre;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float auPlayTime = 1f;

    private bool shaking = false;
    public bool burst = false;
    public bool finalBurst = false;

    private float selfDamage;
    private float rand = 0;

    // Use this for initialization
    void Start()
    {
        if (hpbar == null)
        {
            hpbar = GameObject.FindGameObjectWithTag("HPbar").GetComponent<Slider>();
        }
        if (ausourcePre == null)
        {
            ausourcePre = Resources.Load<GameObject>("Prefabs/Effect/ausourcePre");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //处理血条
        if (hp > 100)
            hp = 100;
        hpbar.value = hp / 100;

    }
    public void Hit(float damage, Vector3 hitPos)
    {
        hp -= damage;
        if (hp < 0 && !burst)
        {
            hp = 0;
            transform.parent.GetComponent<EjectPlayer>().ActiveEject();
            burst = true;
        }
        if (hp <= maxHp * 0.2f)
        {
            if (!burningParticle.isPlaying)
            {
                burningParticle.Play();
            }
        }
        else
        {
            if (burningParticle.isPlaying)
            {
                burningParticle.Stop();
            }
        }

        selfDamage = damage;
        if (hp > 0)
        {
            StartCoroutine(HitEffect(hitPos));
        }


    }

    IEnumerator HitEffect(Vector3 hitPos)
    {

        GameObject tempAudio = Instantiate(ausourcePre);
        tempAudio.GetComponent<AudioSource>().clip = hitSound;
        tempAudio.GetComponent<AudioSource>().Play();
        Destroy(tempAudio, auPlayTime);

        GameObject tempParticle = Instantiate(hitParticlePre, hitPos, Quaternion.Euler(0, 0, 0), transform);
        yield return new WaitForSeconds(hitEffectTime);
        Destroy(tempParticle);
    }


}
