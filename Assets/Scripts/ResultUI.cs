using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public static ResultUI instance;
    public GameObject resultPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI rewardText;

    void Awake() => instance = this;

    public void ShowResult(bool isWin, int reward)
    {
        resultPanel.SetActive(true);
        titleText.text = isWin ? "MISSION COMPLETE" : "MISSION FAILED";
        rewardText.text = $"REWARD: {reward} Gold";
    }

    // '로비로 돌아가기' 버튼에 연결하세요.
    public void GoToLobby()
    {
        // 로비 씬의 정확한 이름을 입력하세요.
        SceneManager.LoadScene("LobbyScene");
    }
}