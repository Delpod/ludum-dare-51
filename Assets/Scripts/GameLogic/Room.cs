using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] Gates[] gatesList;
    [SerializeField] Transform monsters;

    public float orthoSize = 5.6f;

    [HideInInspector] public int roomDifficulty = 10;
    [HideInInspector] public int numberOfMonsters;
    [HideInInspector] public int monstersKilled;

    public void Activate() {
        numberOfMonsters = monsters.childCount;
        for (int i = 0; i < numberOfMonsters; ++i) {
            monsters.GetChild(i).gameObject.SetActive(true);
        }

        monstersKilled = 0;
        CheckIfAllKilled();
    }

    public void CreateMonsters() {
        // TODO: Generate monsters
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
