using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_MenuBox;
    [SerializeField] private GameObject m_SoundBox;

    private void Awake()
    {
        BackToMenu();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenOptions()
    {
        m_MenuBox.SetActive(false);
        m_SoundBox.SetActive(true);
    }

    public void BackToMenu()
    {
        m_MenuBox.SetActive(true);
        m_SoundBox.SetActive(false);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
