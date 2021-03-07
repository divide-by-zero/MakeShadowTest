using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class MyUtil
{
    //渡された重み付け配列からIndexを得る
    public static int GetRandomIndex(params int[] weightTable)
    {
        var totalWeight = weightTable.Sum();
        var value = Random.Range(1, totalWeight + 1);
        var retIndex = -1;
        for (var i = 0; i < weightTable.Length; ++i)
        {
            if (weightTable[i] >= value)
            {
                retIndex = i;
                break;
            }
            value -= weightTable[i];
        }
        return retIndex;
    }

    private static float sTime;

    public static void TimeMeasureStart()
    {
        sTime = Time.realtimeSinceStartup;
    }

    public static float TimeMeasureEnd(bool isDebugLog = false)
    {
        var t = sTime;
        sTime = Time.realtimeSinceStartup;
        if (isDebugLog)
        {
            Debug.Log("time:" + (sTime - t));
            sTime = Time.realtimeSinceStartup;
        }
        return sTime - t;
    }

    private static Texture2D screenShotTexture;
    public static Texture2D ScreenShotTexture { get { return screenShotTexture ?? (screenShotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false)); } }
    private static Coroutine coroutine;
    public static void CaptureScreen(this MonoBehaviour go)
    {
        if (coroutine != null) return;
        coroutine = go.StartCoroutine(_CaptureFrame());
    }
    private static IEnumerator _CaptureFrame()
    {
        int splitCount = 1;
        var splitW = Screen.width / splitCount;
        var splitH = Screen.height / splitCount;
        for (int x = 0; x < splitCount; ++x)
        {
            for (int y = 0; y < splitCount; ++y)
            {
                yield return new WaitForEndOfFrame();
                ScreenShotTexture.ReadPixels(new Rect(x * splitW, y * splitH, splitW, splitH), x * splitW, y * splitH, false);
            }
        }
        yield return null;
        ScreenShotTexture.Apply(false);
        coroutine = null;
    }

    public static Rect CreateNormalizeRect(Vector2 p1, Vector2 p2)
    {
        return CreateNormalizeRect(p1.x, p1.y, p2.x, p2.y);
    }
    public static Rect CreateNormalizeRect(float x1, float y1, float x2, float y2)
    {
        return Rect.MinMaxRect(Mathf.Min(x1, x2), Mathf.Min(y1, y2), Mathf.Max(x1, x2), Mathf.Max(y1, y2));
    }

}

public static class LinqExtensions
{
    public static T RandomAt<T>(this IEnumerable<T> ie)
    {
        if (ie.Any() == false) return default(T);
        return ie.ElementAt(Random.Range(0, ie.Count()));
    }
}

public static class ColorExtensions
{
    public static Color ToDark(this Color col, float mag)
    {
        return new Color(Mathf.Clamp(col.r * mag,0,1), Mathf.Clamp(col.g * mag, 0, 1), Mathf.Clamp(col.b * mag, 0, 1));
    }

    /// <summary>
    /// 渡された色が背景として、見やすい色(白or黒)を返却する
    /// </summary>
    /// <param name="bgColor"></param>
    /// <returns></returns>
    public static Color GetEnableColor(this Color bgColor)
    {
        var grayScale = (bgColor.r * 299 + bgColor.g * 587 + bgColor.b * 114) / 1000;
        return grayScale > 127 ? Color.black : Color.white;
    }
}

public static class DictionaryExtensions
{
    /// <summary>
    /// 値を取得、keyがなければデフォルト値を設定し、デフォルト値を取得
    /// </summary>
    public static TV GetOrDefault<TK, TV>(this Dictionary<TK, TV> dic, TK key, TV defaultValue = default(TV))
    {
        TV result;
        return dic.TryGetValue(key, out result) ? result : defaultValue;
    }
}

public static class ImageExtension
{
    public static Image SetColor(this Image image, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        var c = image.color;
        if (r.HasValue) c.r = r.Value;
        if (g.HasValue) c.g = g.Value;
        if (b.HasValue) c.b = b.Value;
        if (a.HasValue) c.a = a.Value;
        image.color = c;
        return image;
    }

    public static Image AddColor(this Image image, float r = 0.0f, float g = 0.0f, float b = 0.0f, float a = 0.0f)
    {
        var c = image.color;
        c.r += r;
        c.g += g;
        c.b += b;
        c.a += a;
        image.color = c;
        return image;
    }

    public static SpriteRenderer SetColor(this SpriteRenderer sprite, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        var c = sprite.color;
        if (r.HasValue) c.r = r.Value;
        if (g.HasValue) c.g = g.Value;
        if (b.HasValue) c.b = b.Value;
        if (a.HasValue) c.a = a.Value;
        sprite.color = c;
        return sprite;
    }

    public static SpriteRenderer AddColor(this SpriteRenderer sprite, float r = 0.0f, float g = 0.0f, float b = 0.0f, float a = 0.0f)
    {
        var c = sprite.color;
        c.r += r;
        c.g += g;
        c.b += b;
        c.a += a;
        sprite.color = c;
        return sprite;
    }
}

public static class TweenExtensions
{
    public static Sequence ToSequence(this Tweener tween)
    {
        return DOTween.Sequence().Append(tween);
    }

    public static Sequence Append(this Tweener tween, Tweener appendTween)
    {
        return tween.ToSequence().Append(appendTween);
    }

    public static Sequence Join(this Tweener tween, Tweener joinTween)
    {
        return tween.ToSequence().Join(joinTween);
    }

    public static Sequence Prepend(this Tweener tween, Tweener prependTween)
    {
        return tween.ToSequence().Prepend(prependTween);
    }

    public static Sequence Insert(this Tweener tween,float atPosition, Tweener insertTween)
    {
        return tween.ToSequence().Insert(atPosition,insertTween);
    }

    public static Sequence AppendCallback(this Tweener tween, TweenCallback callback)
    {
        return tween.ToSequence().AppendCallback(callback);
    }

    public static Sequence PrependCallback(this Tweener tween, TweenCallback callback)
    {
        return tween.ToSequence().PrependCallback(callback);
    }

    public static Sequence InsertCallback(this Tweener tween, float atPosition, TweenCallback callback)
    {
        return tween.ToSequence().InsertCallback(atPosition, callback);
    }

    public static Sequence AppendInterval(this Tweener tween, float interval)
    {
        return tween.ToSequence().AppendInterval(interval);
    }

    public static Sequence PrependInterval(this Tweener tween, float interval)
    {
        return tween.ToSequence().PrependInterval(interval);
    }
}