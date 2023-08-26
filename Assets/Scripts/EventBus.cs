using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour {
    public static EventBus instance = null;

    public event Action<LevelManager> OnTimerStart = delegate {};
    public event Action<RockGrid> OnFloorCleared = delegate {};

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void TriggerOnTimerStart(LevelManager lm) {
        OnTimerStart?.Invoke(lm);
    }

    public void TriggerOnFloorCleared(RockGrid rg) {
        OnFloorCleared?.Invoke(rg);
    }
}
