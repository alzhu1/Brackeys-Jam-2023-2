using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private float startTime;
    [SerializeField] private float timerBlockGain;

    private float timer;
    public float Timer { get { return timer; } }

    private int floor;
    public int Floor { get { return floor; } }

    private bool started;

    void Start() {
        EventBus.instance.OnFloorCleared += ReceiveFloorClearedEvent;
        EventBus.instance.OnBlockDestroyed += ReceiveBlockDestroyedEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnFloorCleared -= ReceiveFloorClearedEvent;
        EventBus.instance.OnBlockDestroyed -= ReceiveBlockDestroyedEvent;
    }

    void Update() {
        if (!started && Input.GetKeyDown(KeyCode.R)) {
            floor = 1;
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

    void ReceiveFloorClearedEvent(RockGrid rg) {
        floor++;
        EventBus.instance.TriggerOnFloorUpdate(this);
    }

    void ReceiveBlockDestroyedEvent(bool isTimer) {
        if (isTimer) {
            timer += timerBlockGain;
        }
    }
}
