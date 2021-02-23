using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpin : MonoBehaviour
{
    private Quaternion targetRot;
	void Start ()
	{
	    GetComponent<SpinDetecter>().MouseRotation = f =>
	    {
	        if (DragTarget.IsDrag) return;  //ドラッグ中はSpinを受け取らない
	        targetRot = transform.rotation * Quaternion.Euler(0,0,f * 10);
	    };
	}

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRot,0.1f);
    }
}
