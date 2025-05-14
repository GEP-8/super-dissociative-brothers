
using System;
using UnityEngine;

public class MoveRight : MonoBehaviour {
    [SerializeField]
    public float speed = 0.1f;
    
    private void Update() {
        this.transform.Translate(speed * Time.deltaTime, 0f, 0f);
    }
}