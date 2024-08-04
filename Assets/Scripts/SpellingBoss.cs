using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellingBoss : MonoBehaviour
{
    private static SpellingBoss _instance;
    public static SpellingBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpellingBoss>();
            }
            return _instance;
        }
    }

    //LETTER POOL Text Object
    public TextMeshProUGUI m_LetterPoolArea;
    //Spelling area text object
    public TextMeshProUGUI m_SpellingArea;
    //progress bar object
    public Image m_ProgressBar;

    float MAX_SPELLING_TIME = 30f;
    float m_SpellTimeCounter;
    bool m_SpellStarted = false;

    List<char> m_LetterPoolReference = new List<char>();
    List<char> m_TEMP_LETTER_POOL = new List<char>();

    string m_WordBeingSpelled;

    // Update is called once per frame
    void Update()
    {
        if( GameStateBoss.Instance.m_CurrentState == GameStateBoss.PlayStates.spell && m_SpellStarted )
        {
            if( Input.GetKeyDown(KeyCode.Escape) )
            {
                Debug.Log("Quit spelling");
                StopSpelling();
            }
            m_SpellTimeCounter -= Time.deltaTime;
            m_ProgressBar.rectTransform.sizeDelta = new Vector2(m_SpellTimeCounter/MAX_SPELLING_TIME, 100);

            
            //we do stuff in update once the player hits the spelling button
            //start timer
            //record input & visualize

            foreach(char c in Input.inputString )
            {
                if( c == '\b' ) //backspace/delete was pressed
                {
                    if( m_WordBeingSpelled.Length != 0)
                    {
                        //this is great for making a word smaller, but doesn't handle putting a letter back in the pool
                        Debug.Log("letter being deleted: " + m_WordBeingSpelled.ToCharArray()[m_WordBeingSpelled.Length-1]);
                        m_TEMP_LETTER_POOL.Add(m_WordBeingSpelled.ToCharArray()[m_WordBeingSpelled.Length-1]);
                        
                        m_WordBeingSpelled = m_WordBeingSpelled.Substring(0, m_WordBeingSpelled.Length - 1);

                    }
                }
                else if (( c == '\n') || (c == '\r')) //enter/return
                {
                    //check if it is a valid word in the dictonary
                    if( m_WordBeingSpelled.Length > 2 )
                    {
                        CheckWord();
                    }
                }
                else
                {
                    if( m_TEMP_LETTER_POOL.Count > 0 )
                    {
                        foreach (char poolChar in m_TEMP_LETTER_POOL)
                        {
                            if (c.ToString().ToUpper().ToCharArray()[0] == poolChar.ToString().ToUpper().ToCharArray()[0])
                            {
                                m_WordBeingSpelled = m_WordBeingSpelled + c.ToString().ToUpper().ToCharArray()[0];
                                m_TEMP_LETTER_POOL.Remove(poolChar);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Pressin' keys your game can't cash");
                    }
                    
                }
            }

            m_SpellingArea.text = m_WordBeingSpelled;

            //for each key the player presses we must:
                //check if that letter is in the list of letters
                    //if yes, highlight it, add it to the spelled word, remove it from a temporary list
                    //if no, negative feedback
                    //if the player hits enter/taps submit, check word validity
                        //if valid word, send word in for deletion
                        //increase score
                        //check for level increase based on score
                    //if the player hits backspace, delete characters accordingly
                        //let them hold it for multiple deletes?
            //end spell mode
            if (m_SpellTimeCounter <= 0f)
            {
                m_SpellStarted = false;
                StopSpelling();
            }
        }
    }

    public void CheckWord()
    {
        if( DictionaryLogic.Instance.ValidateWord(m_WordBeingSpelled) )
        {
            Debug.LogFormat("{0} is a good word, do stuff.", m_WordBeingSpelled);

            //make a list of objects that were used in spelling the word
                //whoever has the list of objects needs to be able to return the objects that were used in the word
                //who has the list of objects? PlayerController.cs (this might need to be fixed)
            

            LetterPoolBoss.Instance.RemoveLetters(m_WordBeingSpelled.ToCharArray());
            GameStateBoss.Instance.RemoveLettersFromPlayer(m_WordBeingSpelled.ToCharArray());
            LevelAndScoreBoss.Instance.ScoreWord(m_WordBeingSpelled);
            StopSpelling();
        }
        else
        {
            Debug.Log("BAD WORD");
        }
    }

    public void StartSpelling()
    {
        m_WordBeingSpelled = "";
        m_SpellingArea.text = m_WordBeingSpelled;

        m_LetterPoolReference = new List<char>();
        m_TEMP_LETTER_POOL = new List<char>();

        foreach ( char c in LetterPoolBoss.Instance.GetLetterPool() )
        {
            m_LetterPoolReference.Add(c);
            m_TEMP_LETTER_POOL.Add(c);
        }
        
        m_LetterPoolArea.text = m_LetterPoolReference.ToString();
        UpdateLetterPool();
        

        m_SpellStarted = true;
        m_SpellTimeCounter = MAX_SPELLING_TIME;
        m_ProgressBar.rectTransform.localScale = new Vector2(425f*m_SpellTimeCounter / MAX_SPELLING_TIME, 0.25f);
    }

    void UpdateLetterPool()
    {
        string _letterPool = "";
        foreach (char letter in m_LetterPoolReference)
        {
            _letterPool += letter;
        }
        m_LetterPoolArea.text = _letterPool;
    }

    public void StopSpelling()
    {
        //tell game state boss we're done here
        //game state boss must also initiate the cooldown
        GameStateBoss.Instance.StopSpelling();
    }
}
