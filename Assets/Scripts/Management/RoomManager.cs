using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour {
    [SerializeField] Room[] prefabRooms;
    [SerializeField] int minRooms = 4;
    [SerializeField] int maxRooms = 7;

    public List<Room> roomList = new List<Room>();
    [HideInInspector] public Room activeRoom;

    private CinemachineVirtualCamera cmVC;

    private void Start() {
        cmVC = FindObjectOfType<CinemachineVirtualCamera>();
        CreateRooms();
    }

    public void CreateRooms() {
        int roomCount = Random.Range(minRooms, maxRooms + 1);

        for (int i = 0; i < roomCount; ++i) {
            Room prefab = Helpers.RandomElement(prefabRooms);
            Vector3 roomPos = i == 0 ? Vector3.zero : new Vector3(roomList[i - 1].transform.position.x + (roomList[i - 1].size.x + prefab.size.x) / 2f, 0f);
            GameObject go = Instantiate(prefab.gameObject, roomPos, Quaternion.identity, transform);
            go.SetActive(true);
            Room r = go.GetComponent<Room>();
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
        cmVC.Follow = activeRoom.transform;
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
