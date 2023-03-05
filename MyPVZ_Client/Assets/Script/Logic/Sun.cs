using DG.Tweening;
using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    Tween tween;
    public int price;
    private void Start()
    {
        tween = gameObject.transform.DOMoveY(-Random.Range(3, 8), 0.5f, false).SetSpeedBased().SetRelative();
    }
    private void OnMouseEnter()
    {
        tween.Kill();
        LevelManager.Instance.SunTotal += price;
        Destroy(gameObject);
    }
    private void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += Time.deltaTime * 45f;
        transform.eulerAngles= rotation;
    }
}
