using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money = 100; // 초기 자본
    public TextMeshProUGUI moneyText;
    public static PlayerStats instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
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