using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // 일시정지 창 (UI Panel)

    void Start()
    {
        // 게임 시작 시에는 창이 꺼져 있어야 함
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    // 일시정지 버튼을 눌렀을 때 호출
    public void Pause()
    {
        pausePanel.SetActive(true);   // 창 띄우기
        Time.timeScale = 0f;         // 게임 멈춤
    }

    // 계속 플레이(Continue) 버튼을 눌렀을 때 호출
    public void Continue()
    {
        pausePanel.SetActive(false);  // 창 닫기
        Time.timeScale = 1f;         // 게임 다시 시작
    }
}