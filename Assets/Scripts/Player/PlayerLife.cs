using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    LifeDisplay lifeDisplay;
    [SerializeField] int lifeCount = 3;
    [SerializeField] int maxLifeCount = 3;

    void OnEnable()
    {
        lifeCount = maxLifeCount;
        lifeDisplay = FindObjectOfType<LifeDisplay>();
        for (int lifeCounter = 0; lifeCounter < lifeCount; lifeCounter++)
        {
            lifeDisplay.AddLife();
        }
    }

    public void Subtract(int damage)
    {
        lifeCount -= damage;
        lifeDisplay.SubtractLife();
    }

    public int GetLifeCount()
    {
        return lifeCount;
    }
}
