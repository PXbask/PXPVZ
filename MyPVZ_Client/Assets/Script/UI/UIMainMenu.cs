using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIMainMenu : MonoBehaviour
{
    public Button adventureBtn;
    public GameObject zombieHand;
    AsyncOperation asop = null;
    private void Awake()
    {
        zombieHand.SetActive(false);
    }

    public void OnClickadventureBtn()
    {
        StartCoroutine(Adventure());   
    }
    IEnumerator Adventure()
    {
        zombieHand.SetActive(true);
        asop = SceneManager.LoadSceneAsync("level");
        asop.allowSceneActivation = false;
        yield return new WaitForSeconds(3f);
        asop.allowSceneActivation = true;
        yield return null;
    }
    public void OnClickExitBtn()
    {
        Application.Quit();
    }
}
