using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour {
    [SerializeField] Room[] prefabRooms;

    public List<Room> roomList = new List<Room>();
    [HideInInspector] public Room activeRoom;

    private CinemachineVirtualCamera cmVC;

    private void Start() {
        cmVC = FindObjectOfType<CinemachineVirtualCamera>();
        CreateRooms();
    }

    public void CreateRooms() {
        // TODO: Generate rooms

        foreach (Room r in roomList) {
            r.CreateMonsters();
        }

        activeRoom = roomList[0];
        activeRoom.Activate();
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
