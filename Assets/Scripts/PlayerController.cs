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
    private Vector2 nonZeroMoveInput; // for use in dragon mode
    private float moveSpeed = 125;
    [SerializeField]
    private GameObject arcAttackPrefab;
    private bool arcAttacking = false;
    private SpriteRenderer sprite;
    private MouseInfo mouseInfo;

    // dragon mode
    [SerializeField]
    private GameObject dragonHead;
    [SerializeField]
    private GameObject dragonBody;
    private bool inDragonMode = false;

    private void Awake()
    {
        // set up input actions
        playerInputActions = new PlayerInputActions();
        moveAction = playerInputActions.Player.Move;
        moveAction.performed += ctx => ReceiveMoveInput(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => ReceiveMoveInput(Vector2.zero);
        fireAction = playerInputActions.Player.Fire;
        fireAction.performed += ctx => ReceiveFireInput();

        moveSpeed = BalanceVariables.playerMoveSpeed;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inDragonMode)
                ExitDragonMode();
            else
                EnterDragonMode();
        }
    }

    private void FixedUpdate()
    {
        if (arcAttacking)
        {
            rb.velocity = Vector2.zero;
        }
        else if (inDragonMode)
        {
            rb.velocity = nonZeroMoveInput * moveSpeed * Time.fixedDeltaTime;
            dragonHead.transform.rotation *= Quaternion.FromToRotation(-dragonHead.transform.right, 
                Vector2.Lerp(-dragonHead.transform.right, nonZeroMoveInput, Time.deltaTime * 15f));
        }
        else
        {
            rb.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
        }
    }
    private void ReceiveMoveInput(Vector2 input)
    {
        moveInput = input;
        if (input != Vector2.zero)
            nonZeroMoveInput = input;
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

        while(arcAttacking)
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
        inDragonMode = true;
        moveSpeed = BalanceVariables.dragonMoveSpeed;
        GetComponent<CircleCollider2D>().enabled = false;
        dragonHead.SetActive(true);
        dragonBody.transform.position = transform.position;
        dragonBody.SetActive(true);
    }

    private void ExitDragonMode()
    {
        moveSpeed = BalanceVariables.playerMoveSpeed;
        GetComponent<CircleCollider2D>().enabled = true;
        dragonHead.SetActive(false);
        dragonBody.SetActive(false);
        inDragonMode = false;
    }
}
