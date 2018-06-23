using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalButton : MonoBehaviour
{
    [SerializeField] private int selfNum;
    public GameObject objectToMove;
    [SerializeField] private Vector3 buttonPos = new Vector3(0, -2, 0);

    [SerializeField] private float distanceScale = 0.1f;
    // Use this for initialization
    void Start()
    {
        objectToMove.GetComponent<ObjectMove>().moving = false;
        objectToMove.GetComponent<ObjectMove>().dir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            //获取第一个触摸事件
            Touch th = Input.GetTouch(0);
            //判断为触摸开始
            if (th.phase == TouchPhase.Began)
            {
                //获取触摸点生成射线
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(th.position.x, th.position.y, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    //得到射线碰到的物体，即触摸时应该选中的物体
                    GameObject obj = hit.transform.gameObject;
                    //如果选中的是本物体
                    if (obj.Equals(gameObject))
                    {
                        objectToMove.GetComponent<ObjectMove>().moving = true;
                        if (selfNum == 0)
                        {
                            objectToMove.GetComponent<ObjectMove>().dir.x = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.x, 1, -1);

                        }
                        else if (selfNum == 1)
                        {
                            objectToMove.GetComponent<ObjectMove>().dir.y = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.y, 1, -1);
                        }
                        else
                        {
                            objectToMove.GetComponent<ObjectMove>().dir.z = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.z, 1, -1);
                        }
                    }

                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                //获取触摸点生成射线
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
                RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000))
                    {
                        //得到射线碰到的物体，即触摸时应该选中的物体
                        GameObject obj = hit.transform.gameObject;
                        //如果选中的是本物体
                        if (obj.Equals(gameObject))
                        {
                            objectToMove.GetComponent<ObjectMove>().moving = true;
                            if (selfNum == 0)
                            {
                                objectToMove.GetComponent<ObjectMove>().dir.x = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.x, 1, -1);

                            }
                            else if (selfNum == 1)
                            {
                                objectToMove.GetComponent<ObjectMove>().dir.y = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.y, 1, -1);
                            }
                            else
                            {
                                objectToMove.GetComponent<ObjectMove>().dir.z = OverFlow((int)objectToMove.GetComponent<ObjectMove>().dir.z, 1, -1);
                            }
                        }

                    }
                
            }
        }

        transform.parent.rotation = Camera.main.transform.rotation;
        transform.parent.position = objectToMove.transform.position + buttonPos;

        float distance = Vector3.Distance(transform.parent.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        transform.parent.localScale = new Vector3(distance * distanceScale, distance * distanceScale, distance * distanceScale);
    }

    private int OverFlow(int parament, int max, int min)
    {
        parament++;
        if (parament > max)
        {
            parament = min;
        }
        if (parament == max)
        {
            Debug.Log("max");
            this.GetComponent<SpriteRenderer>().color = new Color(238f/255, 0, 0);
            Debug.Log("color: " + this.GetComponent<SpriteRenderer>().color);
        }
        else if (parament == min)
        {
            Debug.Log("min");
            this.GetComponent<SpriteRenderer>().color = new Color(102f / 255, 204f / 255, 255f / 255);
            Debug.Log("color: " + this.GetComponent<SpriteRenderer>().color);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = new Color(255f / 255, 255f / 255, 255f / 255);
            Debug.Log("color: " + this.GetComponent<SpriteRenderer>().color);
        }

        return parament;
    }
}
