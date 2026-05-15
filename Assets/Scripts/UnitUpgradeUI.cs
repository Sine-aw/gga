using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUpgradeUI : MonoBehaviour
{
    public static UnitUpgradeUI instance;

    [Header("UI References")]
    public GameObject uiPanel;
    public TextMeshProUGUI unitNameText;
    public Image unitIcon;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellAmountText;
    public Button upgradeButton;

    private Tower targetTower; // 현재 선택된 타워

    void Awake() { instance = this; }

    void Update()
    {
        // targetTower가 이미 파괴되었는지 항상 체크
        if (uiPanel.activeSelf && targetTower == null)
        {
            Hide();
            return;
        }

        // 공백 클릭 시 닫기
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // 클릭한 곳에 유닛이 없다면 닫기
            Invoke("CheckClose", 0.05f);
        }
    }

    public void SetTarget(Tower tower)
    {
        // 기존 사거리 표시 끄기
        if (targetTower != null) targetTower.ShowRange(false);

        targetTower = tower;
        targetTower.ShowRange(true); // 새 유닛 사거리 켜기

        // UI 텍스트 세팅
        unitNameText.text = tower.data.unitName;
        unitIcon.sprite = tower.data.unitSprite;

        UpdateUIStats();

        uiPanel.SetActive(true);
    }

    void UpdateUIStats()
    {
        // 업그레이드 정보 (최대 레벨 체크 필요)
        if (targetTower.currentLevel < targetTower.data.stats.Length)
        {
            int cost = targetTower.data.stats[targetTower.currentLevel].upgradeCost;
            upgradeCostText.text = cost + " G";
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCostText.text = "MAX";
            upgradeButton.interactable = false;
        }

        // 판매 금액 (구매가의 70% 가정)
        int sellAmount = (int)(targetTower.data.unitPrice * 0.7f);
        sellAmountText.text = "+ " + sellAmount + " G";
    }

    public void Upgrade()
    {
        int cost = targetTower.data.stats[targetTower.currentLevel].upgradeCost;
        if (PlayerStats.Money >= cost)
        {
            PlayerStats.Money -= cost;
            targetTower.currentLevel++;
            targetTower.ShowRange(true); // 사거리 원 크기 갱신
            UpdateUIStats();
        }
    }

    public void Sell()
    {
        if (targetTower == null) return;

        // 1. 돈 추가
        int sellAmount = (int)(targetTower.data.unitPrice * 0.7f);
        PlayerStats.Money += sellAmount;

        // 2. 사거리 원 끄기 (에러 방지)
        targetTower.ShowRange(false);

        // 3. 타워 파괴
        GameObject towerToDestroy = targetTower.gameObject;

        // 4. UI 참조 먼저 끊기 (에러 핵심 방지)
        targetTower = null;
        Hide();

        // 5. 실제 오브젝트 제거
        Destroy(towerToDestroy);
    }

    public void Hide()
    {
        if (targetTower != null) targetTower.ShowRange(false);
        uiPanel.SetActive(false);
        targetTower = null;
    }

    void CheckClose()
    {
        // 유닛을 클릭한 게 아니라면 닫기
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null) Hide();
    }
}