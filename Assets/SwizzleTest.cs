using UnityEngine;
using VectorExtensions;

public class SwizzleTest : MonoBehaviour
{
	void Start ()
	{
		var v = new Vector2(1,2);
	    var v2 = v.xxxx();
	    var v3 = v2;
	    Debug.Log("v2=" + v2);
	    Debug.Log("v3=" + v3);
	    var v4 = Vector3.up.xzz();
	    v4.xxx();
	}
}
