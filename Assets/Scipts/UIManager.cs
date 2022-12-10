using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_GameOverCanvasGroup;
    [SerializeField] private GameObject m_GameOverCanvas;
    [SerializeField] private CanvasGroup m_GameCanvasGroup;
    [SerializeField] private GameObject m_GameCanvas;
    [SerializeField] private GameObject m_PauseCanvas;
    [SerializeField] private GameObject m_PauseBox;
    [SerializeField] private GameObject m_SoundBox;
    [SerializeField] private CanvasGroup m_WinGameCanvasGroup;
    [SerializeField] private GameObject m_WinGameCanvas;
    [SerializeField] private Slider m_EffectSlider;

    public static float m_EffectValue { get; private set; }

    float m_Timer;

    public static bool m_IsPause { get; private set; }

    [SerializeField] private float fadeDuration = 1;   

    private void Awake()
    {
        SetInitialSetting();
    }

    void SetInitialSetting()
    {
        m_IsPause = false;

        Time.timeScale = 1;

        SliderValue();

        SetInitialCanvasActive();
        SetInitialCanvasGroupAlpha();
    }

    void SetInitialCanvasActive()
    {
        m_PauseCanvas.SetActive(false);
        m_GameOverCanvas.SetActive(false);
        m_WinGameCanvas.SetActive(false);
        m_GameCanvas.SetActive(true);
    }

    void SetInitialCanvasGroupAlpha()
    {
        m_GameCanvasGroup.alpha = 1;
        m_GameOverCanvasGroup.alpha = 0;
        m_WinGameCanvasGroup.alpha = 0;
    }

    void Update()
    {
        GameEnd();
    }

    void CanvasGroupTimer(CanvasGroup canvasGroup, GameObject canvas)
    {
        m_Timer += Time.deltaTime;

        canvasGroup.alpha = m_Timer / fadeDuration;

        m_GameCanvasGroup.alpha -= m_Timer / fadeDuration;

        m_GameCanvas.SetActive(false);
        canvas.SetActive(true);
    }

    void GameEnd()
    {
        if (RubyController.gameOver)
        {
            CanvasGroupTimer(m_GameOverCanvasGroup, m_GameOverCanvas);
        }
        else if (GameManager.m_FixAllRobots)
        {
            CanvasGroupTimer(m_WinGameCanvasGroup, m_WinGameCanvas);
        }
    }

    void SliderValue()
    {
        m_EffectValue = m_EffectSlider.value;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void SetActiveCanvasOptions(bool pauseBox, bool soundBox)
    {
        m_GameCanvas.SetActive(false);
        m_PauseBox.SetActive(pauseBox);
        m_SoundBox.SetActive(soundBox);
    }

    public void OpenOptions()
    {
        SetActiveCanvasOptions(false, true);
    }

    public void CloseOptions()
    {
        SetActiveCanvasOptions(true, false);
        SliderValue();
    }

    void SetActiveCanvasPause(int timeScale, bool gameCanvas, bool pauseCanvas, bool isPause)
    {
        Time.timeScale = timeScale;
        m_GameCanvas.SetActive(gameCanvas);
        m_PauseCanvas.SetActive(pauseCanvas);
        m_IsPause = isPause;
    }

    public void Pause()
    {
        SetActiveCanvasPause(0, false, true, true);
    }

    public void UnPause()
    {
        SetActiveCanvasPause(1, true, false, false);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(1);
    }
}
