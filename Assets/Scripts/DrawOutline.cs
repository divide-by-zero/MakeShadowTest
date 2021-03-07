#if UNITY_EDITOR
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    [SerializeField]
    private Color color; //枠線の色

    private void OnDrawGizmos()
    {
        //再帰呼び出し用ローカル関数
        void DrawOutline(Transform targetTransform)
        {
            var rt = targetTransform as RectTransform;
            Gizmos.DrawWireCube(targetTransform.position, rt.rect.size * targetTransform.lossyScale);

            //子要素も再起で描画
            foreach (Transform t in targetTransform)
            {
                DrawOutline(t);
            }
        }

        Gizmos.color = color;
        DrawOutline(transform);
    }
}
#endif