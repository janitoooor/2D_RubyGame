using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private static int m_BrokenRobotsCount;
    public static bool m_FixAllRobots { get; private set; }

    void Start()
    {
        SetInitialSetting();
    }

    void SetInitialSetting()
    {
        m_BrokenRobotsCount = FindObjectsOfType<EnemyController>().Length;
        m_FixAllRobots = false;
    }

    public static void CurrentFixRobots(int count)
    {
        m_BrokenRobotsCount -= count;

        if(m_BrokenRobotsCount <= 0)
        {
            m_FixAllRobots = true;
        }
    }
}
