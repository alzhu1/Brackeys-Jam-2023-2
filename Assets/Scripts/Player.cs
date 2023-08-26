using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float reach;
    [SerializeField] private LayerMask rockLayer;

    private Rigidbody2D rb;

    private float horizontal;
    private bool shouldDrill;
    private Vector2 drillDirection;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        EventBus.instance.OnFloorCleared += ReceiveFloorClearedEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnFloorCleared -= ReceiveFloorClearedEvent;
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!shouldDrill) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                shouldDrill = true;
                drillDirection = Vector2.left;
            } else if (Input.GetKeyDown(KeyCode.X)) {
                shouldDrill = true;
                drillDirection = Vector2.down;
            } else if(Input.GetKeyDown(KeyCode.C)) {
                shouldDrill = true;
                drillDirection = Vector2.right;
            }
        }
    }

    void FixedUpdate() {
        Vector2 currVelocity = rb.velocity;
        currVelocity.x = horizontal * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = currVelocity;

        if (shouldDrill) {
            // Check for rock in drill direction
            Rock rock = Physics2D.OverlapCircle(rb.position + drillDirection * (1 + reach), 0.1f, rockLayer)?.GetComponent<Rock>();
            rock?.TakeDamage();

            shouldDrill = false;
            drillDirection = Vector2.zero;
        }
    }

    void ReceiveFloorClearedEvent() {
        // Move player to top again
        transform.position = new Vector3(transform.position.x, 5);
    }
}
