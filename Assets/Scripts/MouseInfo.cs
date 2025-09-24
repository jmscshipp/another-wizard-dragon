using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInfo : MonoBehaviour
{
    [SerializeField]
    private Enemy currentEnemy;

    [SerializeField]
    private LayerMask clickableLayers;

    private void Update()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector3.forward, Mathf.Infinity, clickableLayers);
        Debug.DrawRay(ray, Vector3.forward * 100f, Color.red);
        if (hit)
        {
            // new object hovered over
            if (currentEnemy == null || (hit.collider.gameObject != currentEnemy.gameObject))
            {
                if (currentEnemy != null)
                {
                    currentEnemy.MouserHoverLeave();
                }

                currentEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                currentEnemy.MouseHover();
            }
        }
        // left the previously hovered over object
        else if (currentEnemy != null)
        {
            currentEnemy.MouserHoverLeave();
            currentEnemy = null;
        }
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
