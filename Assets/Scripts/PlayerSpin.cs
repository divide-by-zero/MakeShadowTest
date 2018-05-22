using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpin : MonoBehaviour
{
	void Start ()
	{
	    GetComponent<SpinDetecter>().MouseRotation = f =>
	    {
	        if (DragTarget.IsDrag) return;  //ドラッグ中はSpinを受け取らない
            transform.Rotate(0, 0, f);
	    };
	}
}
