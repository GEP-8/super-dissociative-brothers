using Network;
using UnityEngine;

public class stage2EnterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PlayerNetwork.Instance.ShiftAllowedActions(1);
    }
    private void OnTriggerExit(Collider other) {
        PlayerNetwork.Instance.ShiftAllowedActions(-1);
    }
}
