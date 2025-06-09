using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTriggerZone : MonoBehaviour
{
    public float interval = 1f; // text 표시 간격
    public float repeatDelay = 5f; // 반복 지연 시간
    public float checkDuration = 2f; // 상태 체크 기간
    public float checkInterval = 0.2f; // 체크 주기
    public Text countdownText; // UI 텍스트 (화면 중앙)

    private bool activated = false; // 트리거 활성화 여부
    private Coroutine repeatCoroutine; // 반복 카운트다운 코루틴

    private PlayerCollisionHandler player; // 플레이어


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;

            player = other.GetComponent<PlayerCollisionHandler>();
            if (player != null && repeatCoroutine == null)
            {
                repeatCoroutine = StartCoroutine(RepeatCountdown(player));
            }
        }
    }

    IEnumerator RepeatCountdown(PlayerCollisionHandler player)
    {

        while (!player.animator.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            yield return StartCoroutine(CountdownSequence(player));
            yield return new WaitForSeconds(repeatDelay);
        }

    }

    IEnumerator CountdownSequence(PlayerCollisionHandler player)
    {
        string[] messages = { "3", "2", "1", "숙여!" };

        foreach (string msg in messages)
        {
            countdownText.text = msg;
            countdownText.enabled = true;
            yield return new WaitForSeconds(interval);
        }

        countdownText.enabled = false;

        if (!player.animator.GetBool("isCrouching"))
        {
            player.Die(); // 플레이어 사망 처리
        }

    }

    public void Deactivate()
    {
        activated = false;
        repeatCoroutine = null;
        countdownText.enabled = false; // 텍스트 비활성화
        StopAllCoroutines(); // 모든 코루틴 중지
    }
}
