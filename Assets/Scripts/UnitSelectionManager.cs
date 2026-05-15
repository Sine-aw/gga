using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public EquipSlot[] equipSlots;
    public UnitData[] allUnitDatas; // 인스펙터에서 모든 유닛 데이터를 넣어주세요.

    void Start()
    {
        LoadEquippedState();
    }

    // [수정] void를 bool로 변경하여 성공 여부 반환
    public bool TryEquip(UnitData data)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].currentData == null)
            {
                equipSlots[i].SetUnit(data);

                // 핵심: 저장! 슬롯 번호(i)에 유닛 이름을 저장합니다.
                PlayerPrefs.SetString("EquippedUnit_" + i, data.unitName);
                PlayerPrefs.Save();
                Debug.Log(i + "번 슬롯에 저장됨: " + data.unitName); // 로그로 확인!
                return true;
            }
        }
        return false;
    }

    void LoadEquippedState()
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            string savedName = PlayerPrefs.GetString("EquippedUnit_" + i, "");
            if (!string.IsNullOrEmpty(savedName))
            {
                foreach (UnitData data in allUnitDatas)
                {
                    if (data.unitName == savedName)
                    {
                        // 1. 왼쪽 장착 슬롯 채우기
                        equipSlots[i].SetUnit(data);

                        // 2. 인벤토리 슬롯들 중 이 유닛을 찾아 숨기기
                        InventorySlot[] allInvSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();
                        foreach (var inv in allInvSlots)
                        {
                            if (inv.towerData == data)
                            {
                                inv.gameObject.SetActive(false);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    public void ClearSaveData(int index)
    {
        PlayerPrefs.DeleteKey("EquippedUnit_" + index);
        PlayerPrefs.Save();
    }
}