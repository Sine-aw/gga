using UnityEngine;

public class ShopSlotCheck : MonoBehaviour
{
    public UnitData towerData;

    void OnEnable()
    {
        RefreshSlot();
    }

    // 이 함수를 통해 언제든 자신의 상태를 새로고침합니다.
    public void RefreshSlot()
    {
        if (towerData == null) return;

        // 저장된 데이터 확인
        bool isBought = PlayerPrefs.GetInt(towerData.unitName + "_Unlocked", 0) == 1;
        towerData.isUnlocked = isBought;

        if (isBought)
        {
            // [중요] 자기 자신(슬롯 전체)을 비활성화합니다.
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}