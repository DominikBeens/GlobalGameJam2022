using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    [SerializeField] private Transform visual;
    [SerializeField] private float force = 5f;

    private Rigidbody2D body;

    public void Initialize(Vector3 direction) {
        body = GetComponent<Rigidbody2D>();

        Vector2 scaledForce = direction * GameStateMachine.Instance.WorldMoveSpeed * force * Time.deltaTime;
        body.AddForce(scaledForce, ForceMode2D.Impulse);
    }
}
