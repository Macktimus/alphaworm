using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPoolBoss : MonoBehaviour
{
    private static LetterPoolBoss _instance;
    public static LetterPoolBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LetterPoolBoss>();
            }
            return _instance;
        }
    }

    List<char> m_LetterPool = new List<char>();

    public void AddLetter (char c)
    {
        m_LetterPool.Add(c);
        LevelGUIBoss.Instance.UpdateLetterPool(m_LetterPool);
    }

    public List<char> GetLetterPool()
    {
        return m_LetterPool;
    }

    public void RemoveLetters (char[] chars)
    {
        foreach( char c in chars)
        {
            for(int x = 0; x < m_LetterPool.Count; x++)
            {
                if( c == m_LetterPool[x] )
                {
                    m_LetterPool.Remove(m_LetterPool[x]);
                    x--;
                    break;
                }
            }
        }
        LevelGUIBoss.Instance.UpdateLetterPool(m_LetterPool);
    }
}
