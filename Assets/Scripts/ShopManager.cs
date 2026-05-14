using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public void BuyTower(UnitData data)
    {
        if (data.isUnlocked || PlayerPrefs.GetInt(data.unitName + "_Unlocked", 0) == 1) return;

        // [수정] stats[0].upgradeCost 대신 실제 상점 가격인 shopPrice를 사용합니다.
        if (GameManager.instance.gold >= data.shopPrice)
        {
            GameManager.instance.AddGold(-data.shopPrice);

            // 데이터 저장
            PlayerPrefs.SetInt(data.unitName + "_Unlocked", 1);
            PlayerPrefs.Save();
            data.isUnlocked = true;

            // 모든 상점 슬롯 새로고침
            ShopSlotCheck[] allSlots = Resources.FindObjectsOfTypeAll<ShopSlotCheck>();
            foreach (ShopSlotCheck slot in allSlots)
            {
                slot.RefreshSlot();
            }

            Debug.Log(data.unitName + " 구매 완료");
        }
    }
}