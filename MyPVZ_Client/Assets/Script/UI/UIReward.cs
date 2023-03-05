using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReward : UIWindow
{
    public Image contentImage;
    public Text descri;
    public Button nextBtn;
    public GameObject background;

    public RawImage maskImage;

    public Action OnClickNextBtnA;
    protected override void OnStart()
    {
        base.OnStart();
        PlayPreAnimation();
    }

    private void PlayPreAnimation()
    {
        StartCoroutine(PreAnimation());
    }
    IEnumerator PreAnimation()
    {
        background.SetActive(false);
        maskImage.gameObject.SetActive(true);
        yield return null;
        while(maskImage.color.a < 1)
        {
            Color color = maskImage.color;
            color.a += 0.25f * Time.deltaTime;
            maskImage.color = color;
            yield return null;
        }
        background.SetActive(true);
        while(maskImage.color.a > 0)
        {
            Color color = maskImage.color;
            color.a -= 0.5f * Time.deltaTime;
            maskImage.color = color;
            yield return null;
        }
        maskImage.gameObject.SetActive(false);
    }
    public void OnClickNextBtn()
    {
        if(OnClickNextBtnA!= null)
        {
            OnClickNextBtnA();
        }
    }
}
