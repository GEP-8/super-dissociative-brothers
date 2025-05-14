using UnityEngine;

public class SimpleMove : MonoBehaviour {
    [SerializeField] public float speed = 1f;

    private void Update() {
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(speed * Time.deltaTime, 0f, 0f);

        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(-speed * Time.deltaTime, 0f, 0f);

        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(0f, speed * Time.deltaTime, 0f);

        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(0f, -speed * Time.deltaTime, 0f);
    }
}