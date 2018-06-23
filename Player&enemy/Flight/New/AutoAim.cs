using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoAim : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform enemy;
    [SerializeField] float aimSpeed = 10f;
    [SerializeField] GameObject aimIconPre;
    [SerializeField] GameObject aimIcon;

    [SerializeField] float scanDelay = 3f;
    [SerializeField] float scanDistance = 1000f;
    // Use this for initialization
    void Start()
    {
        Destroy(GameObject.Find("AimIcon(Clone)"));
        aimIconPre = Resources.Load<GameObject>("Prefabs/Effect/UIeffect/AimIcon");

        StartCoroutine(ScanEnemy(scanDelay, scanDistance));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
                i--;
            }
        }
        //mobile
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {//mobile end
                Debug.Log("not UI");
                int i = 0;
                for(i=0;i<enemies.Count;i++)
                {
                    if (Vector2.Distance(Input.GetTouch(0).position, Camera.main.WorldToScreenPoint(enemies[i].transform.position)) < 60f)
                    {
                        enemy = enemies[i].transform;
                        if (aimIcon == null)
                        {
                            Destroy(GameObject.Find("AimIcon(Clone)"));
                            aimIcon = Instantiate(aimIconPre, GameObject.Find("EffectCanvas").transform);
                        }
                        break;
                    }
                }
                if(i==enemies.Count)
                {
                    Destroy(aimIcon);
                    enemy = null;
                }
            }
        }

        if (enemy != null)
        {
            if (Vector3.Dot(transform.forward, enemy.position - transform.position) > 0)
            {
                aimIcon.GetComponent<RectTransform>().localScale = Vector3.one;
                aimIcon.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(enemy.position);
            }
            else
            {
                aimIcon.GetComponent<RectTransform>().localScale = Vector3.zero;
            }
            DIY_Movement.SoftLookAt(this.gameObject, enemy.position, aimSpeed);
        }
        else
        {
            Destroy(aimIcon);
        }

    }

    IEnumerator ScanEnemy(float scanDelay, float scanDistance)
    {
        while (true)
        {
            GameObject[] temp_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp_enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < scanDistance)
                {
                    enemies.Add(enemy);
                }
            }
            yield return new WaitForSeconds(scanDelay);

        }
    }

}
