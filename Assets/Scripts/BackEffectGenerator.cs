using System.Collections;
using UnityEngine;

public class BackEffectGenerator : MonoBehaviour
{
    [SerializeField]
    private BackEffect effectPrefab;

    [SerializeField]
    private int cnt = 100;

    void Start()
    {
        var bottomLeft = new Vector2(-55, -30);
        var topRight = new Vector2(55, 30);

        for (int i = 0; i < cnt; i++)
        {
            var p = new Vector2(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y));
            var effect = BackEffect.PoolInstantiate(effectPrefab);
            effect.transform.SetParent(this.transform);
            effect.transform.localPosition = p;
        }
    }
}