using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private float startTime;

    private float timer;
    public float Timer { get { return timer; } }

    // TODO: Find better way to start tiemr
    private bool startedTimer;

    void Start() {
        timer = startTime;
    }

    void Update() {
        if (!startedTimer) {
            EventBus.instance.TriggerOnTimerStart(this);
            startedTimer = true;
            return;
        }

        if (timer > 0) {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        }
    }
}
