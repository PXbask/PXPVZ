using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Trophy : MonoBehaviour
{
    public GameObject trophy;
    public GameObject awardRay;
    public Action OnClick;
    private void Start()
    {
        awardRay.transform.DORotate(new Vector3(0, 0, 360), 8f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear).SetLoops(-1).SetRelative();
    }
    public void AwardRayEnabled(bool enabled = true)
    {
        awardRay.SetActive(enabled);
    }
    private void OnMouseDown()
    {
        if (OnClick != null) OnClick.Invoke();
    }
}
