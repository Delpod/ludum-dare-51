using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour {
    [SerializeField] Room[] prefabRooms;
    [SerializeField] Room[] prefabMicroRooms;
    [SerializeField] int minRooms = 4;
    [SerializeField] int maxRooms = 7;

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
        int microRoomCount = Random.Range(0, roomCount / 2 - 1);

        for (int i = 0; i < roomCount; ++i) {
            Room prefab = Helpers.RandomElement(prefabRooms);
            Vector3 roomPos = i == 0 ? Vector3.zero : new Vector3(roomList[i - 1].transform.position.x + (roomList[i - 1].size.x + prefab.size.x) / 2f, 0f);
            GameObject go = Instantiate(prefab.gameObject, roomPos, Quaternion.identity, transform);
            go.SetActive(true);
            Room r = go.GetComponent<Room>();
            r.TopOpenable(false);
            r.roomDifficulty = difficulty;

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

        SwitchActiveRoom(roomList[0]);
    }

    public bool SwitchActiveRoom(Room room) {
        if (activeRoom == room) {
            return false;
        }

        activeRoom = room;
        cmVC.Follow = activeRoom.followTransform;
        cmVC.m_Lens.OrthographicSize = activeRoom.orthoSize;

        TilemapCollider2D tilemapCollider2D = activeRoom.GetComponent<TilemapCollider2D>();

        AstarPath.active.data.gridGraph.center = tilemapCollider2D.bounds.center;
        AstarPath.active.data.gridGraph.width = (int)tilemapCollider2D.bounds.size.x;
        AstarPath.active.data.gridGraph.depth = (int)tilemapCollider2D.bounds.size.y;
        AstarPath.active.Scan();

        // TODO: Should be set for whole level and then scanned

        activeRoom.Activate();

        return true;
    }
}
