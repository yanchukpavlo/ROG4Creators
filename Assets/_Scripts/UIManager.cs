using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Audio")]
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Slider sliderSound;
    [SerializeField] Slider sliderMusic;

    [Header("Menu UI")]
    [SerializeField] TextMeshProUGUI textScoreMax;
    [SerializeField] TextMeshProUGUI textWaveMax;

    [Header("Game UI")]
    [SerializeField] TextMeshProUGUI textWave;
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] Slider sliderHP;
    [SerializeField] Slider sliderSwcondWeapon;
    [SerializeField] GameObject buttonRestart;

    int playerHP = 500;
    int score = 0;
    int wave = 0;
    int scoreBest = 0;
    int waveBest = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetupUI();
    }

    public void SetupUI()
    {
        SetSoundVolume();
        SetMusicVolume();
        BestScore();
        BestWave();
        sliderHP.maxValue = playerHP;
        sliderHP.value = playerHP;
    }

    public void UpdateData()
    {
        if (score > scoreBest)
        {
            PlayerPrefs.SetInt("score", score);
        }

        if (wave > waveBest)
        {
            PlayerPrefs.SetInt("wave", wave);
        }

        StartCoroutine(WaitAndDo(2, delegate { buttonRestart.SetActive(true); }));
    }

    public void OpenMyUrl()
    {
        Application.OpenURL("https://www.linkedin.com/in/pavlo-yanchuk-73b6b5200/");
    }

    public void ButtonStartGame()
    {
        EventsManager.instance.GameStart(true);
    }

    public void ButtonRestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    void SetSoundVolume()
    {
        float value = PlayerPrefs.GetFloat("sound", -2);
        sliderSound.value = value;
        mainMixer.SetFloat("sound", value * 10);
    }

    public void SetSoundVolume(Slider slider)
    {
        mainMixer.SetFloat("sound", slider.value * 10);

        PlayerPrefs.SetFloat("sound", slider.value);
    }

    void SetMusicVolume()
    {
        float value = PlayerPrefs.GetFloat("music", -2);
        sliderMusic.value = value;
        mainMixer.SetFloat("music", value * 10);
    }

    public void SetMusicVolume(Slider slider)
    {
        mainMixer.SetFloat("music", slider.value * 10);

        PlayerPrefs.SetFloat("music", slider.value);
    }

    void BestScore()
    {
        scoreBest = PlayerPrefs.GetInt("score", 0);
        textScoreMax.text = scoreBest.ToString();
    }

    void BestWave()
    {
        waveBest = PlayerPrefs.GetInt("wave", 0);
        textWaveMax.text = waveBest.ToString();
    }

    public void UpdatePlayerHP(int value)
    {
        playerHP += value;
        sliderHP.value = playerHP;
    }

    public void AddScore(int value)
    {
        score += value;
        textScore.text = score.ToString();
    }

    public void AddWave()
    {
        wave += 1;
        textWave.text = wave.ToString();
    }

    public void SetSecondWeaponSlider(float value)
    {
        sliderSwcondWeapon.value = value;
    }

    IEnumerator WaitAndDo(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}
