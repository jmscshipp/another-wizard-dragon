using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction moveAction;
    private InputAction fireAction;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField]
    private float moveSpeed = 125;
    [SerializeField]
    private GameObject arcAttackPrefab;
    private bool arcAttacking = false;
    private SpriteRenderer sprite;
    private MouseInfo mouseInfo;

    private void Awake()
    {
        // set up input actions
        playerInputActions = new PlayerInputActions();
        moveAction = playerInputActions.Player.Move;
        moveAction.performed += ctx => ReceiveMoveInput(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => ReceiveMoveInput(Vector2.zero);
        fireAction = playerInputActions.Player.Fire;
        fireAction.performed += ctx => ReceiveFireInput();

        rb = GetComponent<Rigidbody2D>();
        mouseInfo = GetComponent<MouseInfo>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        moveAction.Enable();
        fireAction.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        moveAction.Disable();
        fireAction.Disable();
    }

    private void FixedUpdate()
    {
        if (arcAttacking)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
        }
    }
    private void ReceiveMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void ReceiveFireInput()
    {
        Enemy enemy = mouseInfo.MouseOverEnemy();
        if (enemy != null && !arcAttacking)
        {
            StartCoroutine(ArcAttack(enemy));
        }
    }

    private IEnumerator ArcAttack(Enemy enemy)
    {
        arcAttacking = true;
        GameObject arcAttack = Instantiate(arcAttackPrefab, transform.position, Quaternion.identity);

        while( arcAttacking)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            arcAttack.transform.position = Vector2.MoveTowards(arcAttack.transform.position, enemy.transform.position, 10f * Time.deltaTime);
            if (Vector3.Distance(arcAttack.transform.position, enemy.transform.position) <= 0.1f)
            {
                enemy.TakeHit();
                Destroy(arcAttack);
                arcAttacking = false;
            }
        }
    }

    private void EnterDragonMode()
    {

    }
}
