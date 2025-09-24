using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyProjectilePrefab;
    [SerializeField]
    private GameObject outline;
    private int health;
    private NavMeshAgent navAgent;
    private EnemyGoal goal;
    private HealthbarUI healthbarUI;
    [SerializeField]
    private EnemyType type;
    public enum EnemyType
    {
        Fast,
        Normal,
        Big,
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        healthbarUI = GetComponentInChildren<HealthbarUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        // set stats based on type
        switch (type)
        {
            case EnemyType.Fast:
                health = BalanceVariables.fastEnemyHealth;
                navAgent.speed = BalanceVariables.fastEnemySpeed;
                break;
            case EnemyType.Normal:
                health = BalanceVariables.normalEnemyHealth;
                navAgent.speed = BalanceVariables.normalEnemySpeed;
                break;
            case EnemyType.Big:
                health = BalanceVariables.bigEnemyHealth;
                navAgent.speed = BalanceVariables.bigEnemySpeed;
                break;
        }

        healthbarUI.Setup(health);
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGoal(EnemyGoal newGoal)
    {
        goal = newGoal;
        navAgent.SetDestination(goal.transform.position);
    }

    public void TakeHit()
    {
        health--;
        healthbarUI.ChangeHealth(-1);
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        MouseInfo.Instance().UpdateCurrentEnemy(this);
        SetHighlight(true);
    }

    private void OnMouseExit()
    {
        MouseInfo.Instance().UpdateCurrentEnemy(null);
        SetHighlight(false);
    }

    private void SetHighlight(bool activated)
    {
        if (activated)
            outline.GetComponent<SpriteRenderer>().color = Color.white;
        else
            outline.GetComponent<SpriteRenderer>().color = Color.black;
    }

    private IEnumerator Attack()
    {
        switch (type)
        {
            case EnemyType.Fast:
                while (true)
                {
                    Vector2 oldPos = transform.position;
                    yield return new WaitForSeconds(BalanceVariables.fastEnemyAttackRate);
                    float angle = Mathf.Atan2(transform.position.y - oldPos.y, transform.position.x - oldPos.x) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));
                    Shoot(rot, BalanceVariables.fastEnemyProjectileSpeed);
                }
            case EnemyType.Normal:
                while (true)
                {
                    yield return new WaitForSeconds(BalanceVariables.normalEnemyAttackRate);
                    float angle = Mathf.Atan2(goal.transform.position.y - transform.position.y, goal.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));
                    Shoot(rot, BalanceVariables.normalEnemyProjectileSpeed);
                }
            case EnemyType.Big:
                while (true)
                {
                    yield return new WaitForSeconds(BalanceVariables.bigEnemyAttackRate);
                    // up down left right
                    Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 180f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 270f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    // diagonals
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 45f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 135f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 225f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                    rot = Quaternion.Euler(new Vector3(0f, 0f, 315f));
                    Shoot(rot, BalanceVariables.bigEnemyProjectileSpeed);
                }
        }
    }

    private void Shoot(Quaternion direction, float speed)
    {
        EnemyProjectile newProjectile = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        newProjectile.SetRotation(direction);
        newProjectile.SetSpeed(speed);
    }
}
