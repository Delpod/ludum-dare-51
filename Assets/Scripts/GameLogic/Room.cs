using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour {
    [SerializeField] Gates gatesTop;
    [SerializeField] Gates gatesLeft;
    [SerializeField] Gates gatesRight;
    [SerializeField] Transform monsters;
    [SerializeField] int maxDifficulty = -1;
    [SerializeField] Vortex vortexPrefab;
    [SerializeField] AudioClip doorOpenClip;

    public Transform followTransform;
    public Vector2Int size;
    public float orthoSize = 5.6f;

    [HideInInspector] public bool lastRoom = false;
    [HideInInspector] public int roomDifficulty = 10;
    public int numberOfMonsters;
    [HideInInspector] public int monstersKilled;

    private Tilemap tilemap;
    private WaitForSeconds doorOpenWait = new(0.25f);

    private void Awake() {
        tilemap = GetComponent<Tilemap>();
    }

    public void Activate() {
        numberOfMonsters = monsters.childCount;
        for (int i = 0; i < numberOfMonsters; ++i) {
            monsters.GetChild(i).gameObject.SetActive(true);
        }

        monstersKilled = 0;
        StartCoroutine(CheckIfAllKilled());
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
        StartCoroutine(CheckIfAllKilled());
    }

    private IEnumerator CheckIfAllKilled() {
        CameraShake.instance.StartConstantShaking(1f);

        yield return doorOpenWait;

        if (monstersKilled == numberOfMonsters) {
            gatesTop.Open();

            if (gatesLeft) {
                gatesLeft.Open();
            }

            if (gatesRight) {
                gatesRight.Open();
            }

            GameManager.RoomCleared();
            AudioSource.PlayClipAtPoint(doorOpenClip, transform.position, 1f);

            if (lastRoom) {
                GameObject go = Instantiate(vortexPrefab.gameObject, transform.position, Quaternion.identity, null);
                go.SetActive(true);
            }
        }

        yield return doorOpenWait;

        CameraShake.instance.StopConstantShaking();
    }

    public void TopOpenable(bool value) {
        gatesTop.openable = value;
    }

    public void LeftOpenable(bool value) {
        gatesLeft.openable = value;
    }

    public void RightOpenable(bool value) {
        gatesRight.openable = value;
    }
}
