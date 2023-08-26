using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGrid : MonoBehaviour {
    [SerializeField] private Rock rockPrefab;

    void Start() {
        
    }

    void Update() {
        // TODO: Remove
        if (Input.GetKeyDown(KeyCode.P)) {
            GenerateLevel(5, 4);
        }
    }

    public void GenerateLevel(int width, int depth) {
        // Every 5 levels, the difficulty should increase?
        // For now, the path can just be the easiest blocks

        // BFS: Start at the top (can just be rock grid y position)

        Vector2Int currPos = new Vector2Int(Random.Range(0, width) - (width / 2), 0);
        HashSet<Vector2Int> rockPositions = new HashSet<Vector2Int>();

        while (currPos.y > -depth) {
            rockPositions.Add(currPos);

            // Pick a random direction
            float chance = Random.Range(0f, 1f);
            if (chance <= 0.33f && !rockPositions.Contains(currPos + Vector2Int.left)) {
                currPos += Vector2Int.left;
            } else if (chance <= 0.67f && !rockPositions.Contains(currPos + Vector2Int.right)) {
                currPos += Vector2Int.right;
            } else {
                currPos += Vector2Int.down;
            }
        }

        // Positions are generated, now instantiate
        for (int y = 0; y > -depth; y--) {
            for (int x = -width / 2; x < (width - width / 2); x++) {
                Rock rock = Instantiate(rockPrefab.gameObject, new Vector3(x, y), Quaternion.identity, transform).GetComponent<Rock>();

                if (rockPositions.Contains(new Vector2Int(x, y))) {
                    rock.SetHp(2);
                } else {
                    rock.SetHp(6);
                }
            }
        }
    }
}
