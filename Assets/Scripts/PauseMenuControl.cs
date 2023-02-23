using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuControl : MonoBehaviour
{
    public Slider musicSlider;
    public Slider ffxSlider;
    public Image manual;
    public bool isOn = false;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        ffxSlider.value = PlayerPrefs.GetFloat("FfxVolume");
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Surrender()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        GameControl.instance.WinLose.gameObject.SetActive(true);
        GameControl.instance.WinLose.transform.GetChild(0).GetComponent<Image>().sprite = GameControl.instance.LoseText;
        for (float i = 0; i < 1; i += 0.1f)
        {
            GameControl.instance.WinLose.GetComponent<CanvasGroup>().alpha = i;
        }
        GameControl.instance.WinLose.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void AudioChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void FFXAudioChange()
    {
        PlayerPrefs.SetFloat("FfxVolume", ffxSlider.value);
    }

    public void ShowManual()
    {
        manual.gameObject.SetActive(true);
    }

    public void CloseManual()
    {
        manual.gameObject.SetActive(false);
    }
}
