using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] private bool useSlider;
    [SerializeField] private Slider slider;

    [SerializeField] private float dragSen = 1f;
    private Vector2 lastDragPos = Vector2.zero;


    private bool isDrag = false;

    private Vector3 offset = Vector3.zero;//偏移量
    [SerializeField] Vector3 defaultPos = Vector3.zero;
    [SerializeField] Vector3 allowedDir = Vector3.one;
    [SerializeField] bool oneDir;
    [SerializeField] float maxMoveRadius = 10f;
    // Use this for initialization
    void Start()
    {
        defaultPos = GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnMouseDown()
    {
        Debug.Log("mouse down");
        lastDragPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (useSlider)
        {
            Debug.Log("Draging");
            slider.value += (Input.mousePosition.y - lastDragPos.y) * dragSen;
            if (slider.value > 1)
            {
                slider.value = 1;
            }
            if (slider.value < 0)
            {
                slider.value = 0;
            }
        }
        else
        {

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (useSlider)
        {
            slider.value += (Input.mousePosition.y - lastDragPos.y) * dragSen;
            if (slider.value > 1)
            {
                slider.value = 1;
            }
            if (slider.value < 0)
            {
                slider.value = 0;
            }
        }
        else
        {
            isDrag = true;
            SetDragObjPostion(eventData);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (useSlider)
        {
            lastDragPos = Input.mousePosition;
        }
        else
        {
            isDrag = false;
            SetDragObjPostion(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (useSlider)
        {

        }
        else
        {
            SetDragObjPostion(eventData);
        }
    }

    void SetDragObjPostion(PointerEventData eventData)
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 mouseWorldPosition;

        //判断是否点到UI图片上的时候
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out mouseWorldPosition))
        {
            if (isDrag)
            {
                if (Vector3.Distance(mouseWorldPosition + offset, defaultPos) <= maxMoveRadius)
                {
                    rect.position = mouseWorldPosition + offset;
                    rect.position = new Vector3((allowedDir.x != 0) ? rect.position.x : defaultPos.x, (allowedDir.y != 0) ? rect.position.y : defaultPos.y, (allowedDir.z != 0) ? rect.position.z : defaultPos.z);
                    if (oneDir)
                    {
                        Vector3 tempVec = rect.position - defaultPos;
                        if (allowedDir.x > 0)
                        {
                            tempVec.x = tempVec.x > 0 ? tempVec.x : 0;
                        }else if(allowedDir.x < 0)
                        {
                            tempVec.x = tempVec.x < 0 ? tempVec.x : 0;
                        }
                        if (allowedDir.y > 0)
                        {
                            tempVec.y = tempVec.y > 0 ? tempVec.y : 0;
                        }
                        else if (allowedDir.y < 0)
                        {
                            tempVec.y = tempVec.y < 0 ? tempVec.y : 0;
                        }
                        
                        rect.position = defaultPos + tempVec;
                    }

                }
            }
            else
            {
                //计算偏移量
                offset = rect.position - mouseWorldPosition;
            }

            //直接赋予position点到的时候回跳动
            //rect.position = mouseWorldPosition;
        }
    }

    /*
    private bool isDrag = false;
    //偏移量
    private Vector3 offset = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = false;
        SetDragObjPostion(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        SetDragObjPostion(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDragObjPostion(eventData);
    }



    void SetDragObjPostion(PointerEventData eventData)
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 mouseWorldPosition;

        //判断是否点到UI图片上的时候
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out mouseWorldPosition))
        {
            if (isDrag)
            {
                rect.position = mouseWorldPosition + offset;
            }
            else
            {
                //计算偏移量
                offset = rect.position - mouseWorldPosition;
            }

            //直接赋予position点到的时候回跳动
            //rect.position = mouseWorldPosition;
        }
    }
    */
}
