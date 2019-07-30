using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private readonly static string[] HUD_WINDOWS_NAMES = new string[] { "MenuWindow", "SoundControlWindow", "LevelCompletedWindow", "OutOfTurnsWindow" };

    [SerializeField]
    private Slider m_musicSlider;
    [SerializeField]
    private Slider m_soundSlider;
    [SerializeField]
    private CanvasGroup m_LevelCompletedWindow;
    [SerializeField]
    private CanvasGroup m_OutOfTurnsWindow;
    public CanvasGroup OutOfTurnsWindow
    {
        get { return m_OutOfTurnsWindow; }
    }
    [SerializeField]
    private Text[] m_scoreValue;
    [SerializeField]
    private Text m_turnsValue;

    private GraphicRaycaster m_raycaster;
    private static HUD m_instance;
    public static HUD Instance
    {
        get { return m_instance; }
    }

    private void Awake()
    {
        m_instance = this;
        m_raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        CloseAllHudWindows();
    }

    private void CloseAllHudWindows()
    {
        foreach (string name in HUD_WINDOWS_NAMES)
        {
            Transform menuWindow = gameObject.transform.Find(name);
            menuWindow.gameObject.SetActive(false);
        }
    }

    public void UpdateTurnsValue(int value)
    {
        m_turnsValue.text = value.ToString();
    }
    public void UpdateScoreValue(int value)
    {
        for(int i = 0; i < m_scoreValue.Length; i++)
        {
            m_scoreValue[i].text = value.ToString();
        }       
    }

    public void ShowWindow(CanvasGroup window)
    {
        window.gameObject.SetActive(true);
    }
    public void HideWindow(CanvasGroup window)
    {
        window.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        Controller.ControllerInstance.Reset();
    }

    public void SetMusicVolume(float volume)
    {
        Controller.ControllerInstance.NewAudio.MusicVolume = volume;
    }
    public void SetSoundVolume(float volume)
    {
        Controller.ControllerInstance.NewAudio.SfxVolume = volume;
    }

    public void UpdateOptions()
    {
        m_musicSlider.value = Controller.ControllerInstance.NewAudio.MusicVolume;
        m_soundSlider.value = Controller.ControllerInstance.NewAudio.SfxVolume;
    }

    public void Next()
    {
        Controller.ControllerInstance.InitializeLevel();
    }

    private IEnumerator Count(int to, float delay)
    {
        m_raycaster.enabled = false;
        for(int i = 1; i <= to; i++)
        {
            yield return new WaitForSeconds(delay);
            Controller.ControllerInstance.NewScore.AddTurnBonus();
        }
        DataStore.SaveGame();
        m_raycaster.enabled = true;
    }

    public void CountScore(int to)
    {
        ShowWindow(m_LevelCompletedWindow);
        StartCoroutine(Count(to, 0.3f));
    }

    public void PlayPreviewSound()
    {
        Controller.ControllerInstance.NewAudio.PlaySound("Drop");
    }
}
