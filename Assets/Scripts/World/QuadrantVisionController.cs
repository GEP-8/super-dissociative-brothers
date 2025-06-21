using Unity.Netcode;
using UnityEngine;

public class QuadrantVisionController : MonoBehaviour
{
    public Material quadrantMaterial;
    public int quadrantIndex = 1; // PlayerVisionManager에서 설정됨
    public Transform character;
    public Camera fixedCamera;


    void OnEnable()
    {
        PlayerVisionManager.OnSetupVision += HandleSetupVision;
    }

    void OnDisable()
    {
        PlayerVisionManager.OnSetupVision -= HandleSetupVision;
    }

    void HandleSetupVision()
    {
        // Quadrant 정보는 시작할 때 한 번만 설정
        if (quadrantMaterial)
        {
            quadrantMaterial.SetFloat("_Quadrant", quadrantIndex);
        }
    }

    void LateUpdate()
    {
        if (!quadrantMaterial || !character || !fixedCamera) return;
        if (!NetworkManager.Singleton.IsClient) return;

        // 캐릭터 월드 위치 → Viewport(UV) 좌표로 변환
        Vector3 viewportPos = fixedCamera.WorldToViewportPoint(character.position);
        quadrantMaterial.SetVector("_CharPos", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
    }
}
