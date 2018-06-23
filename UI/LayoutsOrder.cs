using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutsOrder : MonoBehaviour {
    [SerializeField] private int order=0;
    [SerializeField] private bool active = false;
    // Use this for initialization
    void Start () {
        if (active)
        {
            if (!this.GetComponent<Canvas>())
            {
                this.gameObject.AddComponent<Canvas>();
            }
            this.GetComponent<Canvas>().overrideSorting = true;
            this.GetComponent<Canvas>().sortingOrder = order;
        }

    }

}
