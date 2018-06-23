using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Vector3 dir = Vector3.zero;
    public float speed = 1f;
    [SerializeField] private GameObject preMoveButton;


    public bool moving;
    private GameObject moveButton;
    private UniversalButton[] childrenButtons;
    // Use this for initialization
    void Start()
    {
        if (preMoveButton == null)
        {
            preMoveButton = Resources.Load("Prefabs/UI/MoveButton") as GameObject;
        }
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
                        if (moveButton == null)
                        {
                            moveButton = Instantiate(preMoveButton, transform.position, Camera.main.transform.rotation);
                            //moveButton.transform.parent = transform;
                            childrenButtons = moveButton.GetComponentsInChildren<UniversalButton>();
                            for (int i = 0; i < childrenButtons.Length; i++)
                            {
                                childrenButtons[i].objectToMove = this.gameObject;
                            }
                        }
                    }
                    else
                    {
                        if (obj.tag != "MoveButton")
                        {
                            if (moveButton != null)
                            {
                                childrenButtons = null;
                                Destroy(moveButton);

                            }
                        }
                    }
                }
                else
                {
                    if (moveButton != null)
                    {
                        childrenButtons = null;
                        Destroy(moveButton);

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
                        if (moveButton == null)
                        {
                            moveButton = Instantiate(preMoveButton, transform.position, Camera.main.transform.rotation);
                            //moveButton.transform.parent = transform;
                            childrenButtons = moveButton.GetComponentsInChildren<UniversalButton>();
                            for (int i = 0; i < childrenButtons.Length; i++)
                            {
                                childrenButtons[i].objectToMove = this.gameObject;
                            }
                        }
                    }
                    else
                    {
                        if (obj.tag != "MoveButton")
                        {
                            if (moveButton != null)
                            {
                                childrenButtons = null;
                                Destroy(moveButton);

                            }
                        }
                    }
                }
                else
                {
                    if (moveButton != null)
                    {
                        childrenButtons = null;
                        Destroy(moveButton);

                    }
                }

            }
        }

        if (moving)
        {
            if (!Physics.Raycast(transform.position, dir, 0.1f))
            {
                transform.Translate(Vector3.Normalize(dir) * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                transform.Translate(-Vector3.Normalize(dir) * speed * Time.deltaTime);
                if (moveButton != null)
                {
                    childrenButtons = null;
                    Destroy(moveButton);

                }
            }
        }



    }
    private void OnCollisionEnter(Collision collision)
    {
        moving = false;
        transform.Translate(-Vector3.Normalize(dir) * speed * Time.deltaTime);
    }
}
