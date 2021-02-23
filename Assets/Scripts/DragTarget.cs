using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTarget : MonoBehaviour
{
    private static int dragCount = 0;

    public static bool IsDrag => dragCount > 0;

    private void OnMouseDrag()
    {
        var distance = Vector3.Dot(Camera.main.transform.forward, transform.position - Camera.main.transform.position);
        var mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,distance);
        var objPos = Camera.main.ScreenToWorldPoint(mousePos);
        GetComponent<Rigidbody>().velocity = (objPos - transform.position) * 10;
 
    }

    private void OnMouseUp()
    {
        dragCount--;
    }

    private void OnMouseDown()
    {
        dragCount++;
    }
}
