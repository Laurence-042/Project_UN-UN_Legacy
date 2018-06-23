using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSphere : MonoBehaviour
{
    [SerializeField] List<NormalArmor> enemies;
    [SerializeField] float radius = 10f;
    [SerializeField] float damagePerSec = 100f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemies.Clear();
        GameObject[] temp_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (temp_enemies.Length > 0)
        {
            foreach (GameObject temp_enemy in temp_enemies)
            {
                if (temp_enemy.GetComponent<NormalArmor>())
                {
                    enemies.Add(temp_enemy.GetComponent<NormalArmor>());
                }
            }
        }

        foreach (NormalArmor enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < radius)
            {
                enemy.TakeDamage(damagePerSec * Time.deltaTime, enemy.transform.position);
            }
        }
    }
}
