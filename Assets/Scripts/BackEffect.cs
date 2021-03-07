using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class BackEffect : PoolMonoBehaviour<BackEffect>
{
    public Sprite[] sprites;

    SpriteRenderer SpriteRenderer { set; get; }

    void Start ()
	{
	    SpriteRenderer = GetComponent<SpriteRenderer>();
	    SpriteRenderer.SetColor(a:0.2f);
	    SpriteRenderer.sprite = sprites.RandomAt();
        SpriteRenderer.transform.localScale = new Vector3(2,2,2);
        transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
	}

	void Update ()
	{
        transform.AddPosition(Physics2D.gravity * Time.deltaTime * 0.1f);
        transform.Rotate(0,0,Time.deltaTime*5);

        // var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-1.2f, -1.2f));
        // var topRight = Camera.main.ViewportToWorldPoint(new Vector3(2.2f, 2.2f));
        var bottomLeft = new Vector2(-55, -30);
        var topRight = new Vector2(55, 30);

        if (transform.localPosition.x < bottomLeft.x) transform.localPosition += new Vector3((topRight - bottomLeft).x, 0);
        if (transform.localPosition.y < bottomLeft.y) transform.localPosition += new Vector3(0, (topRight - bottomLeft).y);
        if (transform.localPosition.x > topRight.x)   transform.localPosition -= new Vector3((topRight - bottomLeft).x, 0);
        if (transform.localPosition.y > topRight.y)   transform.localPosition -= new Vector3(0, (topRight - bottomLeft).y);

     //
     //    SpriteRenderer.AddColor(a: -Time.deltaTime/30);
	    // if (SpriteRenderer.color.a < 0)
	    // {
	    //     PoolDestroy(this);
	    // }
	}
}