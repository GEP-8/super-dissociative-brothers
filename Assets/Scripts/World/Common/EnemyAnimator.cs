using UnityEngine;

public class EnemyAnimator : SimpleAnimator
{
    public BoxCollider2D col;

    protected override void UpdateSprite(Sprite newSprite)
    {
        base.UpdateSprite(newSprite);
        
        // Update the BoxCollider2D size and offset based on the new sprite (bottom center pivot)
        if (col != null)
        {
            float ppu = newSprite.pixelsPerUnit;
            float width = newSprite.rect.width / ppu;
            float height = newSprite.rect.height / ppu;

            col.size = new Vector2(width, height);
            col.offset = new Vector2(0, height / 2);
        }
    }


}
