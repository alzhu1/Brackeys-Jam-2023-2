using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float reach;
    [SerializeField] private LayerMask rockLayer;

    [SerializeField] private SpriteRenderer leftDrill;
    [SerializeField] private SpriteRenderer downDrill;
    [SerializeField] private SpriteRenderer rightDrill;

    private Rigidbody2D rb;

    private bool canMove;
    private float horizontal;

    private bool shouldDrill;
    private Vector2 drillDirection;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Start() {
        EventBus.instance.OnStart += ReceiveStartEvent;
        EventBus.instance.OnFloorCleared += ReceiveFloorClearedEvent;
        EventBus.instance.OnLose += ReceiveLoseEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnStart -= ReceiveStartEvent;
        EventBus.instance.OnFloorCleared -= ReceiveFloorClearedEvent;
        EventBus.instance.OnLose -= ReceiveLoseEvent;
    }

    void Update() {
        if (!canMove) {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        bool shouldDrillLeft = Input.GetButton("DrillLeft");
        bool shouldDrillDown = !shouldDrillLeft && Input.GetButton("DrillDown");
        bool shouldDrillRight = !shouldDrillLeft && !shouldDrillDown && Input.GetButton("DrillRight");

        shouldDrill = shouldDrillLeft || shouldDrillDown || shouldDrillRight;
        if (shouldDrillLeft) {
            drillDirection = Vector2.left;
        } else if (shouldDrillDown) {
            drillDirection = Vector2.down;
        } else if (shouldDrillRight) {
            drillDirection = Vector2.right;
        } else {
            drillDirection = Vector2.zero;
        }

        leftDrill.enabled = shouldDrillLeft;
        downDrill.enabled = shouldDrillDown;
        rightDrill.enabled = shouldDrillRight;
    }

    void FixedUpdate() {
        if (!canMove) {
            return;
        }

        Vector2 currVelocity = rb.velocity;
        currVelocity.x = horizontal * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = currVelocity;

        if (shouldDrill) {
            // Check for rock in drill direction
            Rock rock = Physics2D.OverlapCircle(rb.position + drillDirection * (1 + reach), 0.1f, rockLayer)?.GetComponent<Rock>();
            rock?.TakeDamage();
        }
    }

    void ReceiveStartEvent(LevelManager lm) {
        canMove = true;
        rb.gravityScale = 1;
        transform.position = new Vector3(0, 22);
    }

    void ReceiveFloorClearedEvent(RockGrid rg) {
        // Move player to top again
        transform.position = new Vector3(transform.position.x, 5);
    }

    void ReceiveLoseEvent(LevelManager lm) {
        canMove = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }
}
