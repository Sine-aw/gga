using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SummonPanelManager : MonoBehaviour
{
    public List<UnitData> allUnitDatas;
    public GameObject summonButtonPrefab;
    public Transform panelParent;

    void Start() { CreateSummonButtons(); }

    public void CreateSummonButtons()
    {
        foreach (Transform child in panelParent) { Destroy(child.gameObject); }

        for (int i = 0; i < 5; i++)
        {
            string savedName = PlayerPrefs.GetString("EquippedUnit_" + i, "");
            if (string.IsNullOrEmpty(savedName)) continue;

            UnitData data = FindDataByName(savedName);
            if (data != null)
            {
                GameObject btnObj = Instantiate(summonButtonPrefab, panelParent);

                // UI 설정
                Image icon = btnObj.transform.Find("Icon").GetComponent<Image>();
                if (icon != null) icon.sprite = data.unitSprite;

                TextMeshProUGUI priceText = btnObj.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
                if (priceText != null) priceText.text = data.unitPrice.ToString();

                // [해결의 핵심] 
                // 버튼 하나당 딱 하나의 데이터만 1:1로 고정시켜주는 코드입니다.
                // 이렇게 하면 46번 줄에서 나던 Null 에러가 물리적으로 불가능해집니다.
                UnitData targetData = data;

                Button btn = btnObj.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    // 이제 targetData는 버튼이 클릭될 때까지 이 안에서 안전하게 보존됩니다.
                    Debug.Log(targetData.unitName + " 클릭됨");
                    if (BuildManager.instance != null)
                    {
                        BuildManager.instance.SelectUnitToBuild(targetData);
                    }
                });
            }
        }
    }

    UnitData FindDataByName(string name)
    {
        return allUnitDatas.Find(x => x.unitName == name);
    }
}