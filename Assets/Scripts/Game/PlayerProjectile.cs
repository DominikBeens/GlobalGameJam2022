using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    [SerializeField] private Transform visual;
    [SerializeField] private float force = 5f;

    private Rigidbody2D body;

    public void Initialize(Vector3 direction, float velocity) {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(direction * velocity * force, ForceMode2D.Impulse);
    }
}
