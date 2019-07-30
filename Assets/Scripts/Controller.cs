using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    #region Variables_and_Properties
    private int m_currentLevel;
    public int CurrentLevel
    {
        get { return m_currentLevel; }
        set { m_currentLevel = value; }
    }

    [SerializeField] private Color[] m_tokenColors;
    public Color[] TokenColors
    {
        get { return m_tokenColors; }
        set { m_tokenColors = value; }
    }

    [SerializeField] private static Controller m_controllerInstance;
    public static Controller ControllerInstance
    {
        get
        {
            if (m_controllerInstance == null)
            {
                var controller = Instantiate(Resources.Load("Prefab/Controller")) as GameObject;
                m_controllerInstance = controller.GetComponent<Controller>();
            }
            return m_controllerInstance;
        }
    }

    private List<List<Token>> m_tokensByTypes;
    public List<List<Token>> TokensByTypes
    {
        get { return m_tokensByTypes; }
        set { m_tokensByTypes = value; }
    }

    private Field m_field;
    public Field NewField
    {
        get { return m_field; }
        set { m_field = value; }
    }

    [SerializeField]
    private LevelParameters m_level;
    public LevelParameters Level
    {
        get { return m_level; }
        set { m_level = value; }
    }

    [SerializeField]
    private Score m_score;
    public Score NewScore
    {
        get { return m_score; }
        set { m_score = value; }
    }

    [SerializeField]
    private Audio m_audio = new Audio();
    public Audio NewAudio
    {
        get { return m_audio; }
        set { m_audio = value; }
    }

    #endregion

    private void Awake()
    {
        if(m_controllerInstance == null)
        {
            m_controllerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(m_controllerInstance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        NewAudio.SourceMusic = gameObject.AddComponent<AudioSource>();
        NewAudio.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        NewAudio.SourceSFX = gameObject.AddComponent<AudioSource>();

        DataStore.LoadOptions();
    }

    private void Start()
    {
        DataStore.LoadGame();
        InitializeLevel();
        NewAudio.PlayMusic(true);
    }

    public void InitializeLevel()
    {
        m_level = new LevelParameters(m_currentLevel);
        TokenColors = MakeColors(m_level.TokenTypes);
        TokensByTypes = new List<List<Token>>();
        for (int i = 0; i < m_level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }
        do
        {
            m_field = Field.Create(m_level.FieldSize, m_level.FreeSpace);
        } while (!IsAllTokensConnected());
        HUD.Instance.UpdateTurnsValue(m_level.Turns);
    }

    private Color[] MakeColors(int count)
    {
        Color[] result = new Color[count];
        float colorStep = 1f / (count + 1);

        float hue = 0f;
        float saturation = 0.6f;
        float value = 1f;

        for(int i = 0; i < count; i++)
        {
            float newHue = hue + (colorStep * i);
            result[i] = Color.HSVToRGB(newHue, saturation, value);
        }
        return result;
    }

    public void TurnDone()
    {       
        if (IsAllTokensConnected())
        {
            NewAudio.PlaySound("Victory");
            m_currentLevel++;
            Destroy(m_field.gameObject);
            NewScore.AddLevelBonus();
            HUD.Instance.CountScore(m_level.Turns);
            return;
        }
        NewAudio.PlaySound("Drop");
        m_level.Turns--;
        if (m_level.Turns <= 0)
        {
            Destroy(m_field.gameObject);
            HUD.Instance.ShowWindow(HUD.Instance.OutOfTurnsWindow);
        }
    }

    public bool IsAllTokensConnected()
    {
        for(var i = 0; i < TokensByTypes.Count; i++)
        {
            if(AreTokensConnected(TokensByTypes[i]) == false)
            {
                return false;
            }
        }
        return true;
    }

    private bool AreTokensConnected(List<Token> tokens)
    {
        if(tokens.Count == 0)
        {
            return true;
        }

        List<Token> connectedTokens = new List<Token>();
        connectedTokens.Add(tokens[0]);
        bool moved = true;

        while (moved)
        {
            moved = false;
            for(int i = 0; i < connectedTokens.Count; i++)
            {
                for(int j = 0; j < tokens.Count; j++)
                {
                    if(AreTokensNear(tokens[j], connectedTokens[i]))
                    {
                        if(connectedTokens.Contains(tokens[j]) == false)
                        {
                            connectedTokens.Add(tokens[j]);
                            moved = true;
                        }
                    }
                }
            }
        }

        if(tokens.Count == connectedTokens.Count)
        {
            return true;
        }

        return false;
    }

    private bool AreTokensNear(Token first, Token second)
    {
        if ((int)first.transform.position.x ==
        (int)second.transform.position.x + 1 ||
        (int)first.transform.position.x ==
        (int)second.transform.position.x - 1)
        {
            if ((int)first.transform.position.y ==
            (int)second.transform.position.y)
            {
                return true;
            }
        }
        if ((int)first.transform.position.y ==
        (int)second.transform.position.y + 1 ||
        (int)first.transform.position.y ==
        (int)second.transform.position.y - 1)
        {
            if ((int)first.transform.position.x ==
            (int)second.transform.position.x)
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        CurrentLevel = 1;
        NewScore.CurrentScore = 0;
        Destroy(m_field.gameObject);
        InitializeLevel();
    }
}
