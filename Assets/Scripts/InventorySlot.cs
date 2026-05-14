using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public UnitData towerData;
    public UnitSelectionManager manager;

    void Awake()
    {
        // 1. 버튼 컴포넌트가 없으면 자동으로 추가
        Button btn = GetComponent<Button>();
        if (btn == null) btn = gameObject.AddComponent<Button>();

        // 2. 클릭 시 실행될 함수 연결
        btn.onClick.AddListener(OnClick);
    }

    void OnEnable()
    {
        // 데이터가 없으면 슬롯을 아예 꺼버립니다 (에러 방지)
        if (towerData == null)
        {
            gameObject.SetActive(false);
            return;
        }
        RefreshStatus();
    }

    public void RefreshStatus()
    {
        if (towerData == null) return;

        // 구매 여부 확인
        bool isBought = PlayerPrefs.GetInt(towerData.unitName + "_Unlocked", 0) == 1;
        if (towerData.unitName == "Archer") isBought = true;

        // 장착 여부 확인 (장착되어 있으면 인벤토리에서 숨김)
        bool isEquipped = false;
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.GetString("EquippedUnit_" + i, "") == towerData.unitName)
            {
                isEquipped = true;
                break;
            }
        }

        gameObject.SetActive(isBought && !isEquipped);
    }

    public void OnClick()
    {
        // 매니저와 데이터가 있을 때만 장착 시도
        if (manager != null && towerData != null)
        {
            bool success = manager.TryEquip(towerData);
            if (success) gameObject.SetActive(false);
        }
    }
}