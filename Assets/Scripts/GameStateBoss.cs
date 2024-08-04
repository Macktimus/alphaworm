using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBoss : MonoBehaviour
{
    private static GameStateBoss _instance;
    public static GameStateBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameStateBoss>();
            }
            return _instance;
        }
    }

    public enum PlayStates { starting = 0, play, spell, pause, gameover};
    public PlayStates m_CurrentState = PlayStates.starting;

    PlayerController m_PlayerReference;

    public bool m_SnakeMovementReference = false;

    public float m_Speed = 1f;
    public float m_StepTime = 1f;
    public float m_StepSize = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cheat for starting game before implementing a proper start function
        if( Input.GetKeyDown(KeyCode.P) && m_CurrentState == PlayStates.starting )
        {
            Debug.Log("Test Spelling Screen");
            m_CurrentState = PlayStates.play;
            LevelAndScoreBoss.Instance.SetupLevel();
        }
        else if( Input.GetKeyDown(KeyCode.Return) && m_CurrentState == PlayStates.play )
        {
            Debug.Log("Test Spelling Screen");
            SwitchToSpelling();
        }
    }

    public void SwitchToSpelling ()
    {
        m_CurrentState = PlayStates.spell;
        LevelGUIBoss.Instance.StartSpelling();
        SpellingBoss.Instance.StartSpelling();
    }

    public void StopSpelling ()
    {
        LevelGUIBoss.Instance.StopSpelling();
        m_CurrentState = PlayStates.play;
        //set spelling cooldown
    }

    public void SetPlayerReference (PlayerController pc)
    {
        m_PlayerReference = pc;
        m_PlayerReference.SetStepSizeType(m_SnakeMovementReference, m_Speed, m_StepTime, m_StepSize);
    }

    public void IncreaseStep( float TimeChange, float MIN_StepTime )
    {
        m_PlayerReference.IncreaseStep(TimeChange, MIN_StepTime);
    }

    public List<Vector3> GetPlayerBodyPartPositions()
    {
        return m_PlayerReference.ReturnBodyPartPositions();
    }

    public void RemoveLettersFromPlayer(char[] charArray)
    {

        //get the objects that were used in the word from the player, and also remove them from the player
        m_PlayerReference.RemoveFromBody(charArray);
    }
    
}
