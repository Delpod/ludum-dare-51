using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ContentList : MonoBehaviour {
    static public ContentList instance { get; private set; }

    [Serializable]
    public struct MonsterEntry {
        public GameObject enemyPrefab;
        public float difficulty;
    }

    public List<MonsterEntry> monsterList = new();
    public List<TileBase> availableTileList = new();

    private void Awake() {
        instance = this;
    }
}
