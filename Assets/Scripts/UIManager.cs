using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private GameObject titleUI;
    [SerializeField] private Text timerText;
    [SerializeField] private Text floorText;
    [SerializeField] private GameObject loseUI;

    private Text loseUIText;

    void Awake() {
        loseUIText = loseUI.GetComponentInChildren<Text>(true);
    }

    void Start() {
        EventBus.instance.OnStart += ReceiveStartEvent;
        EventBus.instance.OnFloorUpdate += ReceiveFloorUpdateEvent;
        EventBus.instance.OnLose += ReceiveLoseEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnStart -= ReceiveStartEvent;
        EventBus.instance.OnFloorUpdate -= ReceiveFloorUpdateEvent;
        EventBus.instance.OnLose -= ReceiveLoseEvent;
    }

    void Update() {
        
    }

    void ReceiveStartEvent(LevelManager lm) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        titleUI.SetActive(false);
        loseUI.SetActive(false);
        floorText.text = "Floor 1";
        StartCoroutine(UpdateTimer(lm));
    }

    void ReceiveFloorUpdateEvent(LevelManager lm) {
        floorText.text = $"Floor {lm.Floor}";
    }

    void ReceiveLoseEvent(LevelManager lm) {
        loseUI.SetActive(true);
        loseUIText.text = $"Time's up!\n\nBest floor: Floor {lm.Floor}\n\nPress R to restart.";
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
