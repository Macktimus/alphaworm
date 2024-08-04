using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGUIBoss : MonoBehaviour
{

    private static LevelGUIBoss _instance;
    public static LevelGUIBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelGUIBoss>();
            }
            return _instance;
        }
    }

    //letter pool text object
    public TextMeshProUGUI m_LetterPoolObject;
    //level text object
    public TextMeshProUGUI m_LevelTextObject;
    //score text object
    public TextMeshProUGUI m_ScoreTextObject;

    //level start object (here)
    //spelling UI object (the logic for this lives in SpellingBoss.cs) but we facilitate turning the screen on here
    public GameObject m_SpellingScreen;
    //pause menu UI object (logic for that will be here)
    //gameover object (here)


    // Start is called before the first frame update
    void Start()
    {
        m_LetterPoolObject.text = "";
        m_ScoreTextObject.text = "0";
        m_LevelTextObject.text = "1";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //reset function
        //pool object
        //score
        //level

    //pause function
        //unpause button
        //settings?
        //quit

    public void StartSpelling()
    {
        m_SpellingScreen.SetActive(true);
    }

    public void StopSpelling()
    {
        m_SpellingScreen.SetActive(false);
    }


    public void UpdateLetterPool(List<char> c)
    {
        string _letterPool = "";
        foreach ( char letter in c )
        {
            _letterPool += letter;
        }
        m_LetterPoolObject.text = _letterPool;
    }

    public void UpdateScore (int i)
    {
        m_ScoreTextObject.text = i.ToString();
    }
    
    
    //spelling ui object
        //letter pool
        //timer bar
        //which letters have been entered

}
