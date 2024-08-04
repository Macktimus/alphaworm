using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DictionaryLogic : MonoBehaviour
{
    private static DictionaryLogic _instance;
    public static DictionaryLogic Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DictionaryLogic>();
            }
            return _instance;
        }
    }

    public List<TextAsset> m_ListOfWordLists = new List<TextAsset>();

    Dictionary<string, string> WordListDict = new Dictionary<string, string>();


    HashSet<string> WordList = new HashSet<string>();

    private void Awake()
    {
        foreach( TextAsset tasset in m_ListOfWordLists )
        {
            foreach (string word in tasset.ToString().Split())
            {
                WordList.Add(word);
            }
        }
        
        Debug.Log(WordList.Count);
        
    }

    public bool ValidateWord (string s)
    {
        int _listIndex = (s.ToCharArray()[0] % 32)-1;
        
        //if(m_ListOfWordLists[_listIndex].text.Contains(s.ToLower()) )
        foreach( string word in WordList )
        {
            if (word == s.ToLower())
            {
                //Debug.Log("Found word.");
                return true;
            }
        }
        //Debug.Log("Not word.");
        return false;
    }
}
