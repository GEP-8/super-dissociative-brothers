using System;
using System.Linq;
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
        if (Network.PlayerInput.Instance) { // 이렇게 하면 안되는데;;;;
            downOn.gameObject.SetActive(Network.PlayerInput.Instance._allowedActions.Contains(PlayerAction.Crouch));
            upOn.gameObject.SetActive(Network.PlayerInput.Instance._allowedActions.Contains(PlayerAction.Jump));
            leftOn.gameObject.SetActive(Network.PlayerInput.Instance._allowedActions.Contains(PlayerAction.LeftMove));
            rightOn.gameObject.SetActive(Network.PlayerInput.Instance._allowedActions.Contains(PlayerAction.RightMove));
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
