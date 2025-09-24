using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoal : MonoBehaviour
{
    [SerializeField]
    private int buildingHealth = 20;
    private HealthbarUI healthbarUI;


    private void Awake()
    {
        healthbarUI = GetComponentInChildren<HealthbarUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthbarUI.Setup(buildingHealth);
    }

    public void TakeHit()
    {
        buildingHealth--;
        healthbarUI.ChangeHealth(-1);
        if (buildingHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
