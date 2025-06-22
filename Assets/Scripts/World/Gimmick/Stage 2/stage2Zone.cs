using Network;
using UnityEngine;

public class stage2EnterZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerNetwork.Instance.ShiftAllowedActions(1);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerNetwork.Instance.ShiftAllowedActions(0);
        }
    }
}
