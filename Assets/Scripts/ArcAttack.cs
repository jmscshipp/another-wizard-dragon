using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcAttack : MonoBehaviour
{
    [SerializeField]
    private Transform innerOutline;
    [SerializeField]
    private Transform outerOutline;

    // Update is called once per frame
    void Update()
    {
        innerOutline.Rotate(0f, 0f, 200f * Time.deltaTime);
        outerOutline.Rotate(0f, 0f, -200f * Time.deltaTime);
    }
}
