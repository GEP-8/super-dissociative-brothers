using UnityEngine;

public class QuadrantVisionController : MonoBehaviour
{
    public Material quadrantMaterial;
    public int quadrantIndex = 1; // 1~4
    public Transform character;
    public Camera fixedCamera;
    

    void Update()
    {
        if (!quadrantMaterial || !character || !fixedCamera) return;

        // 캐릭터 월드 위치 → Viewport(UV) 좌표로 변환
        Vector3 viewportPos = fixedCamera.WorldToViewportPoint(character.position);
        quadrantMaterial.SetFloat("_Quadrant", quadrantIndex);
        quadrantMaterial.SetVector("_CharPos", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
    }
}
