using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterFoodObject : FoodObject
{
    char m_MyLetter;
    public TextMeshPro m_MyLetterBox; 

    private void Awake()
    {
        //m_MyLetter = m_MyLetterBox.text.ToCharArray()[0];
        //m_MyLetterBox.text = m_MyLetter.ToString();
    }

    public void SetLetter (char c)
    {
        m_MyLetter = c;
        m_MyLetterBox.text = m_MyLetter.ToString();
    }

    public override GameObject GetFoodObject ()
    {
        LevelAndScoreBoss.Instance.ConsumedFood(gameObject);
        LetterPoolBoss.Instance.AddLetter(m_MyLetter);
        return gameObject;
        //add it to the end of the snake
    }

    public char GetLetter()
    {
        return m_MyLetter;
    }
}
