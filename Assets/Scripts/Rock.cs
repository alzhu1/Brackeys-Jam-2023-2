using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
    private const int HEALTH_MULTIPLIER = 10;

    private SpriteRenderer sr;

    private int level;
    private int hp = 1;
    private int maxHp = 1;

    private bool timerBlock;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetLevel(int level) {
        this.level = level;
        maxHp = hp = level * 2;

        switch (this.level) {
            case 0: {
                sr.color = Color.black;
                maxHp = hp = 50;
                break;
            }
            case 1: {
                sr.color = Color.green;
                break;
            }

            case 2: {
                sr.color = Color.blue;
                break;
            }

            case 3: {
                sr.color = Color.yellow;
                break;
            }

            case 4: {
                sr.color = Color.red;
                break;
            }

            case 5: {
                sr.color = Color.magenta;
                break;
            }

            default: {
                sr.color = Color.white;
                maxHp = hp = 1;
                break;
            }
        }

        maxHp *= HEALTH_MULTIPLIER;
        hp *= HEALTH_MULTIPLIER;
    }

    public void SetTimer() {
        timerBlock = true;
        maxHp = hp = 4 * HEALTH_MULTIPLIER;
        StartCoroutine(ColorChange());
    }

    public void TakeDamage() {
        hp--;
        Color currColor = sr.color;
        currColor.a = ((float)hp) / maxHp;
        sr.color = currColor;

        if (hp <= 0) {
            if (timerBlock) {
                EventBus.instance.TriggerOnTimerBlockDestroyed();
            }
            Destroy(gameObject);
        }
    }

    IEnumerator ColorChange() {
        Color[] colorList = new Color[]{
            Color.red,
            Color.green,
            Color.blue
        };

        int currColorIndex = 0;
        while (true) {
            Color startColor = colorList[currColorIndex];
            Color endColor = colorList[(currColorIndex + 1) % colorList.Length];

            float t = 0;
            while (t < 1) {
                sr.color = Color.Lerp(startColor, endColor, t);
                yield return null;
                t += Time.deltaTime;
            }
            sr.color = endColor;

            currColorIndex = (currColorIndex + 1) % colorList.Length;
        }
    }
}
