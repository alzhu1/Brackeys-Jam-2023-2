using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private float startTime;

    private float timer;
    public float Timer { get { return timer; } }

    private bool started;

    void Update() {
        if (!started && Input.GetKeyDown(KeyCode.M)) {
            EventBus.instance.TriggerOnStart(this);
            timer = startTime;
            started = true;
            return;
        }

        if (timer > 0) {
            timer = Mathf.Max(timer - Time.deltaTime, 0);

            if (timer <= 0) {
                // Trigger lose condition
                EventBus.instance.TriggerOnLose(this);
                started = false;
            }
        }
    }
}
