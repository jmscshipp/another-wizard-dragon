using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 130f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(Die());
    }

    void FixedUpdate()
    {
        rb.velocity = transform.right * speed * Time.fixedDeltaTime;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyGoal")
        {
            collision.gameObject.GetComponent<EnemyGoal>().TakeHit();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "EnemyGoal")
        {
            Destroy(gameObject);
        }
    }
}
