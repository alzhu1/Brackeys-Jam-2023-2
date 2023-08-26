using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
    private SpriteRenderer sr;

    private int level;
    private int hp = 1;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        
    }

    public void SetLevel(int level) {
        this.level = level;
        this.hp = level * 2;

        switch (this.level) {
            case 0: {
                sr.color = Color.black;
                this.hp = 50;
                break;
            }
            case 1: {
                sr.color = Color.green;
                // hp = 2;
                break;
            }

            case 2: {
                sr.color = Color.blue;
                // hp = 4;
                break;
            }

            case 3: {
                sr.color = Color.yellow;
                // hp = 6;
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
                this.hp = 1;
                break;
            }
        }
    }

    public void TakeDamage() {
        hp--;

        if (hp <= 0) {
            Destroy(gameObject);
        }
    }
}
