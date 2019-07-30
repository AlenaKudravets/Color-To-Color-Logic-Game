using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelParameters
{
    [SerializeField] private int m_fieldSize;

    [SerializeField] private int m_freeSpace;

    [SerializeField] private int m_TokenTypes;

    [SerializeField] private int m_turns;

    public int FieldSize
    {
        get { return m_fieldSize; }
        set { m_fieldSize = value; }
    }
    public int FreeSpace
    {
        get { return m_freeSpace; }
        set { m_freeSpace = value; }
    }
    public int TokenTypes
    {
        get { return m_TokenTypes; }
        set { m_TokenTypes = value; }
    }
    public int Turns
    {
        get { return m_turns; }
        set
        {
            m_turns = value;
            HUD.Instance.UpdateTurnsValue(m_turns);
        }
    }

    public LevelParameters(int currentLevel)
    {
        //Every 4 levels increased by 1
        int fieldIncreaseStep = currentLevel / 4;

        float subStep = (currentLevel / 4f) - fieldIncreaseStep;

        //Start size is 3 by 3
        //Every 4 levels increased by 1
        m_fieldSize = 3 + fieldIncreaseStep;

        //calculate free spaces
        m_freeSpace = (int)(m_fieldSize * (1f - subStep));
        if (m_freeSpace < 1)
        {
            m_freeSpace = 1;
        }

        m_TokenTypes = 2 + (currentLevel / 3);
        if (m_TokenTypes > 10)
        {
            //мmax color types
            m_TokenTypes = 10;
        }

        //Количество ходов, за которые надо успеть закончить уровень,
        //чтобы получить бонус, зависит от остальных параметров:
        m_turns = (((m_fieldSize * m_fieldSize / 2) - m_freeSpace) * m_TokenTypes) + m_fieldSize;
    }

}
