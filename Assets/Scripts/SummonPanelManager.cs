using UnityEngine;
using UnityEngine.UI;

public class SummonPanelManager : MonoBehaviour
{
    public UnitData[] allUnitDatas;   // 프로젝트의 모든 UnitData (검색용)
    public GameObject summonButtonPrefab; // 하단에 생성될 버튼 프리팹
    public Transform panelParent;    // 검은색 UI 패널 (Content 레이아웃)

    void Start()
    {
        CreateSummonButtons();
    }

    void CreateSummonButtons()
    {
        // 1. 기존에 패널에 붙어있던 연습용 버튼들은 제거
        foreach (Transform child in panelParent) { Destroy(child.gameObject); }

        // 2. 저장된 5개의 슬롯 확인
        for (int i = 0; i < 5; i++)
        {
            string savedName = PlayerPrefs.GetString("EquippedUnit_" + i, "");

            if (!string.IsNullOrEmpty(savedName))
            {
                UnitData data = FindDataByName(savedName);
                if (data != null)
                {
                    // 버튼 생성
                    GameObject btnObj = Instantiate(summonButtonPrefab, panelParent);

                    // 버튼 내부의 이미지 컴포넌트 찾아서 유닛 아이콘 넣기
                    // (자식 오브젝트 중 Image가 있는 곳을 찾습니다)
                    Image icon = btnObj.transform.Find("Icon").GetComponent<Image>();
                    if (icon != null) icon.sprite = data.unitSprite;

                    // 버튼 클릭 시 유닛 소환 로직 연결 (나중에 BuildManager와 연동)
                    btnObj.GetComponent<Button>().onClick.AddListener(() => OnClickSummon(data));
                }
            }
        }
    }

    UnitData FindDataByName(string name)
    {
        foreach (var data in allUnitDatas)
        {
            if (data.unitName == name) return data;
        }
        return null;
    }

    void OnClickSummon(UnitData data)
    {
        Debug.Log(data.unitName + " 소환 버튼 클릭됨!");
        // 여기에 소환 모드 활성화 코드를 넣을 예정입니다.
    }
}