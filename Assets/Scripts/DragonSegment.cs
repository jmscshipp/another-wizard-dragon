using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSegment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    [SerializeField]
    private Transform connectedPiece;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        moveInput = connectedPiece.position - transform.position;
        transform.rotation *= Quaternion.FromToRotation(transform.up,
                Vector2.Lerp(transform.up, moveInput, Time.fixedDeltaTime * 15f));
    }

}
