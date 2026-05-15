using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money = 150; // 초기 자본
    public TextMeshProUGUI moneyText;
    public static PlayerStats instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // "StartRuby"라는 이름으로 저장된 값을 가져와서 Money에 할당합니다.
        // 값이 없으면 기본값인 100을 사용합니다.
        Money = PlayerPrefs.GetInt("StartRuby", 150);

        UpdateMoneyUI();
    }

    void Update()
    {
        // 매 프레임 UI를 갱신해서 돈이 깎이면 즉시 보이게 함
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Gold: " + Money.ToString();
        }
    }
}