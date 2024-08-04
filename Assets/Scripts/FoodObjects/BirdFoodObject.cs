using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFoodObject : FoodObject
{
    public override GameObject GetFoodObject()
    {
        LevelAndScoreBoss.Instance.ConsumedFood(gameObject);
        return gameObject;
        //add it to the end of the snake
    }
}
