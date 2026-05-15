using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CastleStats : MonoBehaviour
{
    public static CastleStats instance;

    [Header("Castle Settings")]
    public int health = 100;
    public TextMeshProUGUI healthText;

    [Header("UI Panels")]
    public GameObject gameOverPanel; // 게임 오버 시 활성화할 판넬

    void Awake()
    {
        // 이미 인스턴스가 있다면 파괴하고 새로 들어온 자신을 할당 (중복 방지)
        if (instance == null)
        {
            instance = this;
            // 씬이 바뀌어도 파괴되지 않게 하고 싶다면 (선택사항)
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateHealthUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // 데미지 입는 함수
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        UpdateHealthUI();

        if (health <= 0)
        {
            EndGame();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health.ToString();
        }
    }

    void EndGame()
    {
        Debug.Log("성 파괴! 게임 종료");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // 시간을 멈추고 싶다면 아래 주석 해제
        Time.timeScale = 0f; 
    }

    // 재시작 버튼용 함수
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}