using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour {
    public static EventBus instance = null;

    public event Action<LevelManager> OnStart = delegate {};
    public event Action<RockGrid> OnFloorCleared = delegate {};
    public event Action OnLose = delegate {};

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void TriggerOnStart(LevelManager lm) {
        OnStart?.Invoke(lm);
    }

    public void TriggerOnFloorCleared(RockGrid rg) {
        OnFloorCleared?.Invoke(rg);
    }

    public void TriggerOnLose() {
        OnLose?.Invoke();
    }
}
