using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : MonoState {

    public override void Enter(params object[] data) {
        Debug.Log($"GAME OVER");
    }

    public override void Exit() {
    }

    public override void Tick() {
    }
}
