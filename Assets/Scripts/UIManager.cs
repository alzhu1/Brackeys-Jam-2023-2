using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Text timerText;
    [SerializeField] private Text floorText;

    void Start() {
        EventBus.instance.OnTimerStart += ReceiveTimerStartEvent;
        EventBus.instance.OnFloorCleared += ReceiveFloorClearedEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnTimerStart -= ReceiveTimerStartEvent;
        EventBus.instance.OnFloorCleared -= ReceiveFloorClearedEvent;
    }

    void Update() {
        
    }

    void ReceiveTimerStartEvent(LevelManager lm) {
        StartCoroutine(UpdateTimer(lm));
    }

    void ReceiveFloorClearedEvent(RockGrid rg) {
        floorText.text = $"Floor {rg.Floor}";
    }

    IEnumerator UpdateTimer(LevelManager lm) {
        while (lm.Timer >= 0) {
            int minute = (int)(lm.Timer / 60);
            int second = (int)(lm.Timer % 60);

            string minuteStr = minute < 10 ? $"0{minute}" : $"{minute}";
            string secondStr = second < 10 ? $"0{second}" : $"{second}";
            timerText.text = $"{minuteStr}:{secondStr}";

            yield return null;
        }
    }
}
