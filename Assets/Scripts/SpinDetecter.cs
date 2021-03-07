using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Spin(回転ジェスチャー)検出クラス
/// </summary>
public class SpinDetecter : MonoBehaviour
{
    public int targetMouseButton = 0;
    public Action<int> MouseRotation;
    public bool isDrawGizmo = false;
    public Image rotateImage;
    public bool isFlipEffect = true;

    /// <summary>
    /// コールバック方式じゃなく、Update内などで回転方向を得たい場合用
    /// </summary>
    public int Value { private set; get; }

    private const int listSize = 4;
    private List<Vector2> posList = new List<Vector2>(listSize);
    private Vector2 oldPos;
    private Vector2 logicalCenter;

    private Vector3 targetScale;

    void Start()
    {
        targetScale = transform.localScale;
    }

    void FixedUpdate ()
    {
        Value = 0;
        if (EventSystem.current.currentSelectedGameObject != null) return;  //UI触ってる

        var pos = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown((int)targetMouseButton))
	    {
            posList.Clear();
	        oldPos = pos;
	    }

        if (Input.GetMouseButton((int) targetMouseButton) || true)
        {
            if ((oldPos - pos).magnitude > 0.1f)
            {
                posList.Add(pos);
                while (posList.Count > listSize) posList.RemoveAt(0);

                //過去のマウス位置履歴から、論理的な中心点を得る
                if (posList.Count >= listSize)
                {
                    logicalCenter.x = posList.Average(vector2 => vector2.x);
                    logicalCenter.y = posList.Average(vector2 => vector2.y);

                    var a = oldPos - logicalCenter;
                    var b = pos - logicalCenter;
                    //2D外積はUnityには無いっぽい
                    var cross = a.x * b.y - a.y * b.x;

                    if (cross > 0) Value = 1;
                    else if (cross < 0) Value = -1;
                    else Value = 0;
                }

                oldPos = pos;
            }
        }

        MouseRotation?.Invoke(Value);

        //回転方向告知用画像が指定されている場合は良しなに回してあげる
        if (rotateImage != null)
        {
            // rotateImage.transform.Rotate(0,0,Value * Time.deltaTime * 360);

            rotateImage.transform.DORotate(new Vector3(0, 0, Value * 100), 0.2f).SetRelative();

            if (isFlipEffect)
            {
                if (Value != 0)
                {
                    targetScale = rotateImage.transform.localScale;
                    targetScale.x = Value;
                }
                rotateImage.transform.localScale = Vector3.Lerp(rotateImage.transform.localScale, targetScale, 0.1f);
            }
        }
    }

    /// <summary>
    /// Gizmo表示
    /// </summary>
    void OnDrawGizmos()
    {
        if (isDrawGizmo == false)return;

        for (var index = 0; index < posList.Count-1; index++)
        {
            var v1 = Camera.main.ViewportToScreenPoint(posList[index]);
            var v2 = Camera.main.ViewportToScreenPoint(posList[index+1]);

            Gizmos.DrawLine(v1, v2);
        }
        Gizmos.DrawWireCube(Camera.main.ViewportToScreenPoint(logicalCenter), Vector3.one * 20);
    }
}
