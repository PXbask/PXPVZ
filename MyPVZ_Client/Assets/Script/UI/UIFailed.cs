using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFailed : UIWindow
{
    public Image failedImage;
    protected override void OnStart()
    {
        base.OnStart();
        failedImage.transform.localScale = Vector3.zero;
        StartCoroutine(FailedAnimation());
    }
    IEnumerator FailedAnimation()
    {
        while (failedImage.transform.localScale.x < 1)
        {
            failedImage.transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        LevelManager.Instance.Restart();
    }
}
