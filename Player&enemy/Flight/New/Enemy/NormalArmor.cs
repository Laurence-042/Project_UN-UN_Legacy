using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalArmor : MonoBehaviour
{
    [SerializeField] GameObject hitParticlePre;
    [SerializeField] ParticleSystem burningParticle;
    [SerializeField] float deathTime=3f;
    [SerializeField] float hitEffectTime = 1f;
    public float fullArmor = 100f;
    public float nowArmor = 0f;
    [SerializeField] AudioSource auSource;
    [SerializeField] AudioClip[] provoke;
    [SerializeField] AudioClip death;

    bool burst = false;
    // Use this for initialization
    void Start()
    {
        if (nowArmor == 0)
        {
            nowArmor = fullArmor;
        }
        if (burningParticle == null)
        {
            burningParticle = GetComponentInChildren<ParticleSystem>();
        }
        if (auSource == null)
        {
            auSource = GetComponent<AudioSource>();
        }

    }

    public void TakeDamage(float damage, Vector3 hitPos)
    {
        nowArmor -= damage;
        StartCoroutine(HitEffect(hitPos));
        if (nowArmor <= fullArmor*0.2f)
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

        if (nowArmor <= 0&&!burst)
        {
            Burst();
            burst = true;
        }

    }

    public void Burst()
    {
        auSource.PlayOneShot(death);
        Destroy(this.gameObject, deathTime);
    }

    IEnumerator HitEffect(Vector3 hitPos)
    {
        GameObject tempParticle = Instantiate(hitParticlePre, hitPos, Quaternion.Euler(0, 0, 0), transform);
        yield return new WaitForSeconds(hitEffectTime);
        Destroy(tempParticle);
    }

}
