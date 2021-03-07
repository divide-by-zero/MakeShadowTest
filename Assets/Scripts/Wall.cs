using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<BoxCollider2D>(out var collider))
        {
            if (TryGetComponent<SpriteRenderer>(out spriteRenderer))
            {
                collider.size = spriteRenderer.size;
            }
        }
    }

    private void Update()
    {
        var t = 1 - Mathf.Pow(0.1f, Time.deltaTime/5);
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, t);
    }
}
