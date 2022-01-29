using UnityEngine;

public class PlayerManager : Manager<PlayerManager> {

    [SerializeField] private Player playerPrefab;

    public Player Player { get; private set; }

    public void SpawnPlayer() {
        Player = Instantiate(playerPrefab);
        GameEvents.OnPlayerSpawned.Invoke();
    }
}
