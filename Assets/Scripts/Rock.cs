using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
    [SerializeField] private int hp;

    void Start() {
        
    }

    void Update() {
        
    }

    public void SetHp(int hp) {
        this.hp = hp;
    }

    public void TakeDamage() {
        hp--;

        if (hp <= 0) {
            Destroy(gameObject);
        }
    }
}
