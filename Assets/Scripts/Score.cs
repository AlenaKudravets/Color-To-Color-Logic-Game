using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    [SerializeField]
    private int m_currentScore;

    public int CurrentScore
    {
        get { return m_currentScore; }
        set
        {
            m_currentScore = value;
            HUD.Instance.UpdateScoreValue(m_currentScore);
        }
    }

    [SerializeField]
    private int m_levelScoreBonus;

    [SerializeField]
    private int m_turnScoreBonus;

    public void AddLevelBonus()
    {
        CurrentScore += m_levelScoreBonus;
    }
    public void AddTurnBonus()
    {
        CurrentScore += m_turnScoreBonus;
    }
}
