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
    private GameObject playerGraphics;
    [SerializeField]
    private GameObject dragonHead;
    [SerializeField]
    private GameObject dragonBody;
    [SerializeField]
    private GameObject dragonTurretPrefab;
    [SerializeField]
    private GameObject dragonProjectile;
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

        if (inDragonMode)
        {

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
                Vector2.Lerp(-dragonHead.transform.right, nonZeroMoveInput, Time.fixedDeltaTime * 15f));
        }
        else
        {
            rb.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
            if (moveInput != Vector2.zero)
                sprite.transform.localScale = new Vector3(-Mathf.Sign(moveInput.x), 1f, 1f);
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

    private IEnumerator DragonAttack()
    {
        while (inDragonMode)
        {
            yield return new WaitForSeconds(0.1f);
            Quaternion rot = dragonHead.transform.rotation * Quaternion.FromToRotation(dragonHead.transform.right, nonZeroMoveInput)
                * Quaternion.Euler(0f,  0f, Random.Range(-20f, 20f));
            DragonProjectile newProjectile = Instantiate(dragonProjectile, (Vector2)transform.position + nonZeroMoveInput, rot).GetComponent<DragonProjectile>();
            newProjectile.SetSpeed(BalanceVariables.dragonProjectileSpeed);
        }
    }

    private void EnterDragonMode()
    {
        inDragonMode = true;
        moveSpeed = BalanceVariables.dragonMoveSpeed;
        GetComponent<CircleCollider2D>().enabled = false;
        playerGraphics.SetActive(false);
        dragonHead.SetActive(true);
        dragonBody.transform.position = transform.position;
        dragonBody.SetActive(true);
        StartCoroutine(DragonAttack());
    }

    private void ExitDragonMode()
    {
        moveSpeed = BalanceVariables.playerMoveSpeed;
        GetComponent<CircleCollider2D>().enabled = true;
        playerGraphics.SetActive(true);
        dragonHead.SetActive(false);
        dragonBody.SetActive(false);
        inDragonMode = false;
        Instantiate(dragonTurretPrefab, transform.position, Quaternion.identity);
    }
}
