using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyBoardUI : MonoBehaviour

    // 테스트용 주석입니다.
{
    public Image up;
    public Image down;
    public Image left;
    public Image right;

    private readonly byte defaultAlpha = 166;
    private readonly byte activeAlpha = 255;

    void Update()
    {
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
