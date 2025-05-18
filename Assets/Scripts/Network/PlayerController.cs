using UnityEngine;

namespace Network {
    [RequireComponent(typeof(PlayerNetwork))] // 일단은 이렇게 되어있는데 매니저로 빼는게 좋을려나?
    public class PlayerController : MonoBehaviour {
        private PlayerNetwork playerNetwork;

        private void Awake() {
            playerNetwork = PlayerNetwork.Instance;
        }

        private void Update() {
            foreach ((PlayerAction playerAction, bool value) in playerNetwork.MergedActionStatusDictionary) {
                if (!value) {
                    continue;
                }

                switch (playerAction) {
                    case PlayerAction.RightMove:
                        transform.Translate(Vector3.right * (Time.deltaTime * 10));
                        break;
                    case PlayerAction.LeftMove:
                        transform.Translate(Vector3.left * (Time.deltaTime * 10));
                        break;
                    case PlayerAction.UpMove:
                        transform.Translate(Vector3.up * (Time.deltaTime * 10));
                        break;
                    case PlayerAction.DownMove:
                        transform.Translate(Vector3.down * (Time.deltaTime * 10));
                        break;
                    case PlayerAction.Jump:
                        // TODO
                        break;
                }
            }
        }
    }
}