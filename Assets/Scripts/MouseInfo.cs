using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInfo : MonoBehaviour
{
    private Enemy currentEnemy;

    private static MouseInfo instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public static MouseInfo Instance()
    {
        return instance;
    }

    // called by enemy when mouse is over it
    public void UpdateCurrentEnemy(Enemy newEnemy)
    {
        currentEnemy = newEnemy;
    }

    public Enemy MouseOverEnemy()
    {
        return currentEnemy;
    }
}
