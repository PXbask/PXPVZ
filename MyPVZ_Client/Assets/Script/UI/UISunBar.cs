using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISunBar : MonoBehaviour
{
    public Text text;
    private int sunNum;
    public void Init()
    {
        LevelManager.Instance.OnSunCollected += OnSunCollected;
    }
    void OnSunCollected(int num)
    {
        sunNum = num;
        text.text = sunNum.ToString();
    }
    private void OnDestroy()
    {
        LevelManager.Instance.OnSunCollected -= OnSunCollected;
    }
}
