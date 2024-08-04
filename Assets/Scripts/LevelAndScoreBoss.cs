using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAndScoreBoss : MonoBehaviour
{
    private static LevelAndScoreBoss _instance;
    public static LevelAndScoreBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelAndScoreBoss>();
            }
            return _instance;
        }
    }

    public List<SpellingConfigGroups> m_LevelGroups = new List<SpellingConfigGroups>();
    SpellingConfigGroups m_CurrentSpellingConfigGroup;
    public List<SpellingConfigs> m_CurrentListOfSpellingConfigs = new List<SpellingConfigs>();
    SpellingConfigs m_CurrentLevelConfig;

    //need a pool of injected letters and a pool of objects to spawn


    int m_PlayerScore = 0;
    int m_PlayerLevel = 1;

    //we want to handle taking in words to make scores
    //calling updates in the UI
    //level score thresholds

    int m_NewSpellingConfigscore = 100; //grab this from the level config, when we get it, grab a new level from that pool
    int m_LevelCompleteScore = 100; //used to check if the player goes up a level

    List<Vector3> m_SpawnPoints = new List<Vector3>();

    /// <summary>
    /// Object Spawning stuff, let's maybe not split that and this apart
    /// </summary>
    Vector2 MIN_QUADS = new Vector2(-14f, -18f);
    Vector2 MAX_QUADS = new Vector2(14f, -5f);

    List<char> m_CharList = new List<char>(); //letters we add to the pool when its empty
    List<char> m_CharPool = new List<char>(); //the pool of letter, take one away when it is spawned as a letter

    public GameObject m_LetterFood;
    public GameObject m_BadFood; //idea: bad food has a number and you destroy it by either spelling a word of that length or spelling that many words

    List<GameObject> m_FoodItems = new List<GameObject>(); //gotta track position here too

    List<QuestActors> m_CurrentLevelQuests;
    bool m_LevelComplete;

    public void SetupLevel()
    {
        //set level config
        SetLevelConfig();
        RefreshCharacterPool();
        //setup the bounds for spawning items

        int _xMod = 0;
        for (float x = MIN_QUADS.x; x <= MAX_QUADS.x; x++)
        {
            int _yMod = 0;
            for (float y = MIN_QUADS.y; y <= MAX_QUADS.y; y++)
            {
                m_SpawnPoints.Add(new Vector3(MIN_QUADS.x + (1 * _xMod), 0, MIN_QUADS.y + (1 * _yMod)));
                _yMod++;
            }
            _xMod++;
        }

        //spawn 5 items
        for( int x = 0; x < 5; x++)
        {
            SpawnFood();
        }
        
    }

    public void ScoreWord (string word)
    {
        foreach(char c in word.ToCharArray() )
        {
            m_PlayerScore = m_PlayerScore + (word.Length * 2);
        }
        LevelGUIBoss.Instance.UpdateScore(m_PlayerScore);
        //update player level
        //update spawner
        CheckLevelIncrement();//To Do: remove this and check the level for completion
    }

    void IncrementScore (int i)
    {
        m_PlayerScore = m_PlayerScore + i;
        LevelGUIBoss.Instance.UpdateScore(m_PlayerScore);
        //update player level
        CheckLevelIncrement();//To Do: remove this and check the level for completion
    }
    
    void SetLevelConfig ()
    {
        //level config starts at 0
        m_CurrentSpellingConfigGroup = m_LevelGroups[m_PlayerLevel-1];
        foreach( SpellingConfigs lc in m_CurrentSpellingConfigGroup.m_ConfigGroup)
        {
            m_CurrentListOfSpellingConfigs.Add(lc);
        }

        int _randomLevelGroup = Random.Range(0, m_CurrentListOfSpellingConfigs.Count);
        if( _randomLevelGroup == -1 )
        {
            Debug.LogError("Something gravely awful has happened.");
        }
        m_CurrentLevelConfig = m_CurrentListOfSpellingConfigs[_randomLevelGroup];
        m_CurrentListOfSpellingConfigs.RemoveAt(_randomLevelGroup);

        
    }

    void LoadCharacters()
    {
        if( m_CharList.Count > 0)
        {
            m_CharList.Clear();
        }
        
        //letterpool isnt a common concept so we'll have to make it one i guess
        foreach (char c in m_CurrentLevelConfig.m_LetterPool.ToCharArray())
        {
            m_CharList.Add(c);
        }

    }


    void CheckLevelIncrement()
    {
        if( m_PlayerScore >= m_LevelCompleteScore)
        {
            SetLevelConfig();
            //increase level group config we're using
            //update data as a result
        }
        if( m_PlayerScore >= m_NewSpellingConfigscore )
        {
            
        }
    }

    public void ConsumedFood(GameObject go)
    {
        //when we eat a thing we go faster (whether it has a letter or not)
        GameStateBoss.Instance.IncreaseStep(m_CurrentSpellingConfigGroup.m_StepTimeDecrement, m_CurrentSpellingConfigGroup.m_MinStepTime);
        m_FoodItems.Remove(go);
        SpawnFood();
    }

    //need to do the different objects spawned in here
    void SpawnFood()
    {
        //randomly select object type (letter vs. bad food)
        float _foodType = Random.Range(0f, 100f);

        GameObject _go;

        if (_foodType <= m_CurrentSpellingConfigGroup.m_BadFoodChance)
        {
            _go = Instantiate(m_BadFood, GetSpawnPosition(), Quaternion.identity);
        }
        else
        {
            _go = Instantiate(m_LetterFood, GetSpawnPosition(), Quaternion.identity);
            Debug.Log("Created: " + _go.name);
            SetLetter(_go);
        }

        m_FoodItems.Add(_go);
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 _spawnPos = new Vector3();

        List<Vector3> _spawnPoints = new List<Vector3>();

        foreach( Vector3 vec in m_SpawnPoints)
        {
            _spawnPoints.Add(vec);
        }

        //make a list of bad spawn points (food and player combined)
        List<Vector3> _occupiedSpaces = new List<Vector3>();

        foreach( GameObject go in m_FoodItems )
        {
            _occupiedSpaces.Add(go.transform.position);
        }
        foreach( Vector3 vec in GameStateBoss.Instance.GetPlayerBodyPartPositions() )
        {
            _occupiedSpaces.Add(vec);
        }

        if( _spawnPoints.Count == _occupiedSpaces.Count )
        {
            //add an edge case in case someone's gonna be a bit of a dickhead and try to fill the whole screen
            //the kids call this an easter egg
            Debug.Log("fuck.");
        }


        foreach( Vector3 badVec in _occupiedSpaces)
        {
            _spawnPoints.Remove(badVec);
        }

        int _spawnIndex = Random.Range(0, _spawnPoints.Count);
        _spawnPos = _spawnPoints[_spawnIndex];

        return _spawnPos;
    }
    
    void SetLetter (GameObject go)
    {
        //Debug.Log("Setting Letter");
        int _letterIndex = Random.Range(0, m_CharPool.Count);
        //Debug.Log("Randomed: "+_letterIndex);
        go.GetComponent<LetterFoodObject>().SetLetter(m_CharPool[_letterIndex]);
        m_CharPool.RemoveAt(_letterIndex);
        if(m_CharPool.Count == 0)
        {
            RefreshCharacterPool();
        }
    }

    void RefreshCharacterPool()
    {
        LoadCharacters();
        foreach ( char c in m_CharList)
        {
            m_CharPool.Add(c);
        }
    }

    public void CheckUsedLetterForQuests( List<GameObject> lettersUsed)
    {
        //PlayerController sends us the list of objects used in the word

        //Go through each object and validate it against the current Quests

        //Delete the objects after each pass

        //have a quest object/logic (scriptable object)
        //send the scriptable object the letters (one at a time)
        //send the scriptable object an "End" message when the last letter is sent over
        //TO DO: Create this quest-object. We have something like this but it can be improved or needs to be trialed
    }

    //need to set up injecting both Game Objects and Letters into a level
    //right now we're inheriting a bunch of letters, but not gameobjects themselves
}
