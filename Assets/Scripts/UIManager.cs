using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Text timerText;
    [SerializeField] private Text floorText;
    [SerializeField] private GameObject loseUI;

    private Text loseUIText;

    void Awake() {
        loseUIText = loseUI.GetComponentInChildren<Text>(true);
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
        
    }

    void ReceiveStartEvent(LevelManager lm) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        loseUI.SetActive(false);
        floorText.text = "Floor 1";
        StartCoroutine(UpdateTimer(lm));
    }

    void ReceiveFloorClearedEvent(RockGrid rg) {
        floorText.text = $"Floor {rg.Floor}";
    }

    void ReceiveLoseEvent(LevelManager lm) {
        loseUI.SetActive(true);
        // TODO: This sucks, find better way
        RockGrid rg = FindAnyObjectByType<RockGrid>();
        loseUIText.text = $"Time's up!\n\nBest floor: Floor {rg.Floor}\n\nPress M to restart";
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
