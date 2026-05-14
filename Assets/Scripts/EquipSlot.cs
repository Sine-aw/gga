using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSlot : MonoBehaviour
{
    public UnitData currentData;
    public GameObject visualRoot; // 검은 판 + 아이콘 + 이름이 포함된 부모 오브젝트
    public Image unitImage;
    public TextMeshProUGUI unitNameText;
    public int slotIndex; // 인스펙터에서 0~4로 설정

    public void SetUnit(UnitData data)
    {
        currentData = data;
        unitImage.sprite = data.unitSprite;
        unitNameText.text = data.unitName;
        visualRoot.SetActive(true); // 비주얼 켜기
    }

    public void ClearUnit()
    {
        currentData = null;
        visualRoot.SetActive(false); // 비주얼 끄기
    }

    // [추가] 슬롯 클릭 시 장착 해제
    public void OnClickSlot()
    {
        if (currentData != null)
        {
            // 데이터 삭제
            PlayerPrefs.DeleteKey("EquippedUnit_" + slotIndex);
            PlayerPrefs.Save();

            // 인벤토리 슬롯들 새로고침 (비활성화된 오브젝트도 찾기 위해 FindObjectsOfTypeAll 사용)
            InventorySlot[] allSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();
            foreach (var slot in allSlots)
            {
                if (slot.towerData == currentData)
                {
                    slot.RefreshStatus(); // 이 부분의 이름을 'RefreshStatus'로 변경!
                    break;
                }
            }
            ClearUnit();
        }
    }
}