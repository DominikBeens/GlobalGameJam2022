using UnityEngine;

public class PlayerManager : Manager<PlayerManager> {

    [SerializeField] private Player playerPrefab;

    public Player Player { get; private set; }
    public PlayerCamera PlayerCamera { get; private set; }

    public void SpawnPlayer() {
        Player = Instantiate(playerPrefab);
        PlayerCamera = Player.GetComponentInChildren<PlayerCamera>();
    }
}
