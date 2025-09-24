using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class DragonTurret : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    private Transform target;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (enemiesInRange.Count == 0)
            return;

        target = enemiesInRange[0].transform;
        foreach(Enemy enemy in enemiesInRange)
        {
            if (Mathf.Abs(Vector3.Distance(enemy.transform.position, transform.position)) >
                Mathf.Abs(Vector3.Distance(target.position, transform.position)))
            {
                target = enemy.transform;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemiesInRange.Add(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemiesInRange.Remove(collision.gameObject.GetComponent<Enemy>());
    }

    private IEnumerator Attack()
    {
        while(true)
        {
            yield return new WaitForSeconds(BalanceVariables.dragonTurretAttackRate);
            if (target != null)
            {
                float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, angle));
                Shoot(rot, BalanceVariables.dragonTurretProjectileSpeed);
            }
        }
    }

    private void Shoot(Quaternion direction, float speed)
    {
        Debug.Log("DRAGON, SHOOT");
        DragonProjectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<DragonProjectile>();
        newProjectile.SetRotation(direction);
        newProjectile.SetSpeed(speed);
    }
}
