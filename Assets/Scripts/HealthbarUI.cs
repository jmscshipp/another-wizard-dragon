using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField]
    private GameObject healthbarSegmentPrefab;
    [SerializeField]
    private GameObject healthBarParent;

    public void Setup(int health)
    {
        for (int i  = 0; i < health; i++)
        {
            Instantiate(healthbarSegmentPrefab, healthBarParent.transform);
        }
    }

    public void ChangeHealth(int affector)
    {
        // removing health
        if (affector < 0)
        {
            // find the last enabled segment and disable it
            Image[] segments = healthBarParent.GetComponentsInChildren<Image>();
            for (int i = segments.Length - 1; i >= 0; i--)
            {
                if (segments[i].enabled)
                {
                    segments[i].enabled = false;
                    break;
                }
            }
        }
        // healing
        else if (affector > 0)
        {
            // find the last enabled segment and disable it
            Image[] segments = healthBarParent.GetComponentsInChildren<Image>();
            for (int i = 0; i < segments.Length - 1; i++)
            {
                if (!segments[i].enabled)
                {
                    segments[i].enabled = true;
                    break;
                }
            }
        }
    }
}
