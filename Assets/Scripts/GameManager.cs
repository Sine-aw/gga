using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<UnitData> equippedUnits = new List<UnitData>();

    public static GameManager instance;

    public int gold = 100;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (instance == null) instance = this;

        gold = PlayerPrefs.GetInt("TotalGold", 100);
    }

    void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;

        // 골드가 변할 때마다 실시간으로 저장합니다.
        PlayerPrefs.SetInt("TotalGold", gold);
        PlayerPrefs.Save();

        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
        {
            if (goldText != null) goldText.text = gold.ToString();
        }
    }

    void Update()
    {
        // 숫자 8을 누르면 즉시 골드가 10,000G가 됩니다.
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            gold = 10000;
            PlayerPrefs.SetInt("TotalGold", gold);
            PlayerPrefs.Save();
            UpdateGoldUI();
            Debug.Log("골드 치트 사용: 10,000G");
        }

        // 숫자 9를 누르면 모든 데이터(골드, 구매 내역)를 초기화합니다.
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("모든 데이터 초기화 완료! 게임을 재시작하세요.");
        }
    }
}