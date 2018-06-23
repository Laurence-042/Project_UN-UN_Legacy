using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QTEButton : MonoBehaviour
{
    [SerializeField] int staticIndex;
    public int index;
    [SerializeField] QTE centerQTE;
    // Use this for initialization
    void Start()
    {
        if (centerQTE == null)
        {
            centerQTE = GameObject.FindGameObjectWithTag("KeepPlayer").GetComponentInChildren<QTE>();
        }
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnClick()
    {
        Debug.Log("Button "+ index+" Clicked. ClickHandler.");
        centerQTE.ClickButton(index, staticIndex);


    }


}
