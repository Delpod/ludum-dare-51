using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour {
    [SerializeField] Room[] prefabRooms;
    [SerializeField] Room[] prefabMicrorooms;
    [SerializeField] GameObject[] upgradePrefabs;
    [SerializeField] int minRooms = 4;
    [SerializeField] int maxRooms = 7;
    [Range(0f, 1f)]
    [SerializeField] float microroomProbability = 0.3f;
    [Range(0f, 1f)]
    [SerializeField] float microroomProbabilityUpgrade = 0.5f;

    public int startDifficulty = 7;
    [HideInInspector] public List<Room> roomList = new List<Room>();
    [HideInInspector] public int difficulty;
    [HideInInspector] public Room activeRoom;

    private CinemachineVirtualCamera cmVC;

    private void Start() {
        cmVC = FindObjectOfType<CinemachineVirtualCamera>();
        difficulty = startDifficulty;
    }

    public void CreateRooms(bool startAgain = false) {
        foreach(Room r in roomList) {
            Destroy(r.gameObject);
        }

        roomList = new List<Room>();
        activeRoom = null;
        Player.RestartPlayer(startAgain);
        if (startAgain) {
            Timer.RestartTimer();
        }

        int roomCount = Random.Range(minRooms, maxRooms + 1);
        int minX, maxX, minY, maxY;

        List<Room> microRoomsList = new();

        // This is a mess

        for (int i = 0; i < roomCount; ++i) {
            Room prefab = Helpers.RandomElement(prefabRooms);
            Vector3 roomPos = i == 0 ? Vector3.zero : new Vector3(roomList[i - 1].transform.position.x + (roomList[i - 1].size.x + prefab.size.x) / 2f, 0f);
            GameObject go = Instantiate(prefab.gameObject, roomPos, Quaternion.identity, transform);
            go.SetActive(true);
            Room r = go.GetComponent<Room>();
            r.roomDifficulty = difficulty;

            if (i != 0 && i != roomCount -1 && Random.Range(0f, 1f) < microroomProbability) {
                Room microPrefab = Helpers.RandomElement(prefabMicrorooms);
                roomPos += new Vector3(0f, (r.size.y + microPrefab.size.y) / 2f + 1f, 0f);
                go = Instantiate(microPrefab.gameObject, roomPos, Quaternion.identity, transform);
                go.SetActive(true);
                Room micro = go.GetComponent<Room>();

                if (!GameManager.hadFirstUpgrade || microroomProbabilityUpgrade < Random.Range(0f, 1f)) {
                    GameObject upgradePrefab = Helpers.RandomElement(upgradePrefabs);
                    Instantiate(upgradePrefab, micro.followTransform.transform.position, Quaternion.identity, null);
                    GameManager.hadFirstUpgrade = true;
                } else {
                    micro.roomDifficulty = difficulty / 2;
                    micro.CreateMonsters();
                }
                microRoomsList.Add(micro);
            } else {
                r.TopOpenable(false);
            }

            if (i == 0) {
                r.roomDifficulty = 0;
                r.LeftOpenable(false);
            } else if (i == roomCount - 1) {
                r.roomDifficulty += 5;
                r.lastRoom = true;
                r.RightOpenable(false);
            }

            r.CreateMonsters();
            roomList.Add(r);
        }

        minX = (int)roomList[0].followTransform.position.x - roomList[0].size.x / 2;
        maxX = (int)roomList[roomList.Count - 1].followTransform.position.x + roomList[roomList.Count - 1].size.x / 2;
        minY = (int)roomList[0].followTransform.position.y - roomList[0].size.y / 2;
        maxY = (int)roomList[0].followTransform.position.y + roomList[0].size.y / 2;

        roomList.AddRange(microRoomsList);

        foreach (Room r in roomList) {
            int y1 = (int)r.followTransform.position.y - r.size.y / 2;
            int y2 = (int)r.followTransform.position.y + r.size.y / 2;
            if (y1 < minY) {
                minY = y1;
            }

            if (y2 > maxY) {
                maxY = y2;
            }
        }

        SwitchActiveRoom(roomList[0]);

        AstarPath.active.data.gridGraph.center = new Vector3((minX + maxX) / 2f, Mathf.Round((minY + maxY) / 2f), 0f);
        AstarPath.active.data.gridGraph.SetDimensions(maxX - minX, maxY - minY, 1);
        StartCoroutine(ScanMap());
    }

    public bool SwitchActiveRoom(Room room) {
        if (activeRoom == room) {
            return false;
        }

        activeRoom = room;
        cmVC.Follow = activeRoom.followTransform;
        cmVC.m_Lens.OrthographicSize = activeRoom.orthoSize;

        activeRoom.Activate();

        return true;
    }

    private IEnumerator ScanMap() {
        yield return null;
        AstarPath.active.Scan();
    }
}
