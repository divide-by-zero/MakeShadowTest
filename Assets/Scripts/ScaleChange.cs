using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class ScaleChange : MonoBehaviour
{
    [SerializeField]
    private float minScale;

    [SerializeField]
    private float maxScale;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private float targetSize;

    private void Start()
    {
        targetSize = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        targetSize -= Input.mouseScrollDelta.y * Time.deltaTime * 100;
        if (targetSize < minScale) targetSize = minScale;
        if (targetSize > maxScale) targetSize = maxScale;

        var t = 1 - Mathf.Pow(0.1f, Time.deltaTime);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, t);
    }
}
