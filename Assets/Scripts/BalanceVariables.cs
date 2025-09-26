using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalanceVariables : MonoBehaviour
{
    // enemy variables
    public static int fastEnemyHealth = 1;
    public static float fastEnemySpeed = 1.2f; // fast enemy
    public static float fastEnemyAttackRate = 1f;
    public static float fastEnemyProjectileSpeed = 150f;

    public static int normalEnemyHealth = 3;
    public static float normalEnemySpeed = 0.75f; // normal enemy
    public static float normalEnemyAttackRate = 2f;
    public static float normalEnemyProjectileSpeed = 130f;

    public static int bigEnemyHealth = 8;
    public static float bigEnemySpeed = 0.3f; // big enemy
    public static float bigEnemyAttackRate = 0.5f;
    public static float bigEnemyProjectileSpeed = 70f;

    // other

    public static float dragonTurretAttackRate = 2f;
    public static float dragonTurretProjectileSpeed = 130f;
    public static float dragonTurretProjectileLifetime = 1f;

    public static float playerMoveSpeed = 125f;
    public static float dragonMoveSpeed = 250f;
    public static float dragonProjectileSpeed = 300f;
}
