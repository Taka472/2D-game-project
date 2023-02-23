using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Image settingMenu;
    public AudioSource bgm;
    public AudioSource ffx;
    public Slider musicSlider;
    public Slider ffxSlider;
    public Image manual;
    public bool isOn = false;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        ffxSlider.value = PlayerPrefs.GetFloat("FfxVolume");
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        isOn = false;
    }

    private void Update()
    {
        bgm.volume = PlayerPrefs.GetFloat("MusicVolume");
        ffx.volume = PlayerPrefs.GetFloat("FfxVolume");
    }

    public void OnPlayClick()
    {
        ffx.PlayOneShot(ffx.clip);
        SceneManager.LoadScene(1);
    }

    public void SettingClick()
    {
        ffx.PlayOneShot(ffx.clip);
        settingMenu.gameObject.SetActive(true);
    }

    public void Back()
    {
        ffx.PlayOneShot(ffx.clip);
        settingMenu.gameObject.SetActive(false);
    }

    public void DeckEdit()
    {
        ffx.PlayOneShot(ffx.clip);
        SceneManager.LoadScene(2);  
    }

    public void Quit()
    {
        ffx.PlayOneShot(ffx.clip);
        Application.Quit();
    }

    public void AudioChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void ShowManual()
    {
        if (!isOn)
        {
            ffx.PlayOneShot(ffx.clip);
            manual.gameObject.SetActive(true);
            isOn = true;
        }
        else
        {
            ffx.PlayOneShot(ffx.clip);
            manual.gameObject.SetActive(false);
            isOn = false;
        }
    }
}
