using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환 필수 네임스페이스

public class StageSelectManager : MonoBehaviour
{
    // 버튼 클릭 시 호출할 함수
    public void LoadStage(string sceneName)
    {
        // 어떤 스테이지를 선택했는지 인게임에서 알 수 있도록 저장
        PlayerPrefs.SetString("CurrentStage", sceneName);
        PlayerPrefs.Save();

        // 실제 씬 로드
        SceneManager.LoadScene(sceneName);
    }
}