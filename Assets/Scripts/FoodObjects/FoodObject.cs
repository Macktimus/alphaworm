using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public virtual GameObject GetFoodObject ()
    {
        LevelAndScoreBoss.Instance.ConsumedFood(gameObject);
        return gameObject;
    }
}
