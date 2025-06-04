using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    public Sprite[] frames; // array of sprites to animate
    public float frameRate = 0.5f;
    protected SpriteRenderer sr;
    protected int index;
    protected float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            index = (index + 1) % frames.Length; // loop through frames
            UpdateSprite(frames[index]);
            timer = 0f; // reset timer
        }
    }

    protected virtual void UpdateSprite(Sprite newSprite)
    {
        sr.sprite = newSprite; // update the sprite renderer with a new sprite
    }
}
