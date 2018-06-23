using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTest : MonoBehaviour {
    [SerializeField] GameObject Soldier;
    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePositionOnScreen = Input.mousePosition;
        mousePositionOnScreen.z = screenPosition.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);

        if (Input.GetMouseButtonDown(0))
        {

            Instantiate(Soldier, mousePositionInWorld, Quaternion.identity);

        }

    }

}
