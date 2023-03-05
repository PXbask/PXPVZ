using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartTitle : MonoBehaviour
{
    const float LOADING_TIME = 3f;
    AsyncOperation asop = null;
    public Image loadingBar;
    public Image roll;
    public Button startBtn;
    public void StartLoading()
    {
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        asop = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("main");
        asop.allowSceneActivation = false;
        startBtn.gameObject.SetActive(false);
        roll.gameObject.SetActive(true);
        float progress = 0;
        float len = 407 + 375;
        float oldProgress = 0;
        yield return DataManager.Instance.LoadData();
        UserData.Instance.Init();
        while (progress <= 1)
        {
            //progress = op.progress;
            progress += Time.deltaTime / LOADING_TIME;
            loadingBar.fillAmount = progress;

            Vector3 position = roll.transform.parent.position;
            position.x += len * (progress - oldProgress);
            roll.transform.parent.position = position;

            Vector3 scale = roll.transform.parent.localScale;
            float sc = scale.x - (progress - oldProgress) * 0.7f;
            scale.x = sc;scale.y = sc;
            roll.transform.parent.localScale = scale;

            Vector3 rotation = roll.transform.eulerAngles;
            rotation.z -= (progress - oldProgress) * 810f;
            roll.transform.eulerAngles = rotation;

            oldProgress = progress;
            yield return null;
        }
        startBtn.gameObject.SetActive(true);
        roll.gameObject.SetActive(false);
    }
    public void OnClickStartBtn()
    {
        asop.allowSceneActivation = true;
    }
}
