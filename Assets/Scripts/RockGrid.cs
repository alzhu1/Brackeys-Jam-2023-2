using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGrid : MonoBehaviour {
    [SerializeField] private int width;
    [SerializeField] private Rock rockPrefab;

    private EdgeCollider2D winTrigger;
    private List<Rock> rocks;

    private LevelManager lm;

    public int Floor { get { return lm != null ? lm.Floor : 1; } }

    void Awake() {
        winTrigger = GetComponent<EdgeCollider2D>();
        rocks = new List<Rock>();
    }

    void Start() {
        EventBus.instance.OnStart += ReceiveStartEvent;
        EventBus.instance.OnFloorUpdate += ReceiveFloorUpdateEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnStart -= ReceiveStartEvent;
        EventBus.instance.OnFloorUpdate -= ReceiveFloorUpdateEvent;
    }

    void ReceiveStartEvent(LevelManager lm) {
        this.lm = lm;
        GenerateLevel();
    }

    void ReceiveFloorUpdateEvent(LevelManager lm) {
        GenerateLevel();
    }

    int GetHalfWidthForFloor() {
        return Mathf.Min(2 + (lm.Floor / 4), width / 2);
    }

    public void GenerateLevel() {
        foreach (Rock rock in rocks) {
            if (rock != null) {
                Destroy(rock.gameObject);
            }
        }
        rocks.Clear();

        int depth = 4 + lm.Floor / 5;

        // Every 5 levels, the difficulty should increase?
        // For now, the path can just be the easiest blocks

        // BFS: Start at the top (can just be rock grid y position)
        // TODO: Depending on floor, set minX and maxX respectively

        int minX = -GetHalfWidthForFloor();
        int maxX = GetHalfWidthForFloor();
        Vector2Int currPos = new Vector2Int(Random.Range(minX, maxX + 1), 0);

        HashSet<Vector2Int> rockPositions = new HashSet<Vector2Int>();
        List<Vector2Int> availableNextPos = new List<Vector2Int>();

        while (currPos.y > -depth) {
            rockPositions.Add(currPos);

            // Pick a random direction
            availableNextPos.Clear();
            availableNextPos.Add(currPos + Vector2Int.down);

            Vector2Int leftPos = currPos + Vector2Int.left;
            Vector2Int rightPos = currPos + Vector2Int.right;

            if (leftPos.x >= minX && leftPos.x <= maxX && !rockPositions.Contains(leftPos)) {
                availableNextPos.Add(leftPos);
            }

            if (rightPos.x >= minX && rightPos.x <= maxX && !rockPositions.Contains(rightPos)) {
                availableNextPos.Add(rightPos);
            }

            currPos = availableNextPos[Random.Range(0, availableNextPos.Count)];
        }

        // Positions are generated, now instantiate
        for (int y = 0; y > -depth; y--) {
            for (int x = -width / 2; x < (width - width / 2); x++) {
                Rock rock = Instantiate(rockPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Rock>();
                rock.transform.localPosition = new Vector3(x, y);

                if (rockPositions.Contains(new Vector2Int(x, y))) {
                    rock.SetLevel(1);
                } else if (x < minX || x > maxX) {
                    rock.SetLevel(0);
                } else {
                    rock.SetLevel(Random.Range(2, 6));
                }
                rocks.Add(rock);
            }
        }

        // Add a win trigger below the grid (probably say 2 units below)
        float lowestY = transform.position.y - depth;
        List<Vector2> points = new List<Vector2>{
            new Vector2(-width / 2, lowestY - 8),
            new Vector2(width / 2, lowestY - 8)
        };
        winTrigger.SetPoints(points);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Beat this floor");
        EventBus.instance.TriggerOnFloorCleared(this);
    }
}
