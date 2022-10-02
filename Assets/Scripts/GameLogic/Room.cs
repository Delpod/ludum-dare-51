using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour {
    [SerializeField] Gates[] gatesList;
    [SerializeField] Transform monsters;

    public float orthoSize = 5.6f;

    [HideInInspector] public int roomDifficulty = 10;
    [HideInInspector] public int numberOfMonsters;
    [HideInInspector] public int monstersKilled;

    private Tilemap tilemap;

    private void Awake() {
        tilemap = GetComponent<Tilemap>();
    }

    public void Activate() {
        numberOfMonsters = monsters.childCount;
        for (int i = 0; i < numberOfMonsters; ++i) {
            monsters.GetChild(i).gameObject.SetActive(true);
        }

        monstersKilled = 0;
        CheckIfAllKilled();
    }

    public void CreateMonsters() {
        List<Vector2Int> possiblePositions = new();

        BoundsInt bounds = tilemap.cellBounds;
        for (int i = bounds.x; i < bounds.xMax; ++i) {
            for (int j = bounds.y; j < bounds.yMax; ++j) {
                TileBase tileRule = tilemap.GetTile(new Vector3Int(i, j, 0));

                if (ContentList.instance.availableTileList.Contains(tileRule)) {
                    possiblePositions.Add(new Vector2Int(i, j));
                }
            }
        }

        int currentDifficulty = 0;

        while (currentDifficulty + 2 < roomDifficulty) {
            ContentList.MonsterEntry me = Helpers.RandomElement(ContentList.instance.monsterList);
            Vector2Int position = Helpers.RandomElement(possiblePositions);
            possiblePositions.Remove(position);
            GameObject enemy = Instantiate(me.enemyPrefab, new Vector3(transform.position.x + position.x, transform.position.y + position.y), Quaternion.identity, monsters);
            enemy.SetActive(false);
            currentDifficulty += me.difficulty;
        }
    }

    public void MonsterKilled() {
        ++monstersKilled;
        CheckIfAllKilled();
    }

    private void CheckIfAllKilled() {
        if (monstersKilled == numberOfMonsters) {
            foreach (Gates g in gatesList) {
                g.Open();
            }
        }

    }
}
