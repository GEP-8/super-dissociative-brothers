using Network;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyBoardUI : MonoBehaviour
{
    public Image up;
    public Image upOn;
    public Image down;
    public Image downOn;
    public Image left;
    public Image leftOn;
    public Image right;
    public Image rightOn;

    private readonly byte defaultAlpha = 166;
    private readonly byte activeAlpha = 255;
    
    void Update() {
        Debug.LogWarning(Network.PlayerInput.Instance);
        if (Network.PlayerInput.Instance) { // 이렇게 하면 안되는데;;;;
            foreach (PlayerAction action in Network.PlayerInput.Instance._allowedActions) {
                switch (action) {
                    case PlayerAction.Crouch:
                        downOn.gameObject.SetActive(true);
                        break;  
                    case PlayerAction.Jump:
                        upOn.gameObject.SetActive(true);
                        break;
                    case PlayerAction.LeftMove:
                        leftOn.gameObject.SetActive(true);
                        break;
                    case PlayerAction.RightMove:
                        rightOn.gameObject.SetActive(true);
                        break;
                }
            }
        }
        
        if (Keyboard.current == null) return;

        SetAlpha(up, Keyboard.current.upArrowKey.isPressed);
        SetAlpha(down, Keyboard.current.downArrowKey.isPressed);
        SetAlpha(left, Keyboard.current.leftArrowKey.isPressed);
        SetAlpha(right, Keyboard.current.rightArrowKey.isPressed);
    }

    void SetAlpha(Image img, bool isPressed)
    {
        if (img == null) return;

        Color color = img.color;
        color.a = (isPressed ? activeAlpha : defaultAlpha) / 255f;
        img.color = color;
    }
}
