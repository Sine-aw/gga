using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private UnitData unitToBuild;
    private GameObject previewObject;
    private SpriteRenderer previewRangeRenderer;

    public bool IsBuildingMode() { return previewObject != null; }

    [Header("Settings")]
    public LayerMask groundLayer;
    public float minDistanceBetweenUnits = 1.2f;
    public int maxUnitCount = 12;
    private List<GameObject> deployedUnits = new List<GameObject>();

    void Awake() { instance = this; }

    void Update()
    {
        if (previewObject != null)
        {
            // 1. 마우스 좌표 계산 (기존 로직 유지)
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 mPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 spawnPos = new Vector3(mPos.x, mPos.y, 0f);
            previewObject.transform.position = spawnPos;

            bool canBuild = CheckCanBuild(spawnPos);

            if (previewRangeRenderer != null)
                previewRangeRenderer.color = canBuild ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

            // 2. [수정] 클릭 감지: UI가 가리고 있어도 배치를 시도하게 변경
            if (Input.GetMouseButtonDown(0))
            {
                // 진짜 '버튼'이나 '패널' 같은 UI 위인지 다시 확인
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    // [비상 조치] 로그가 뜬다면, 일단 무시하고 배치를 진행해봅니다.
                    // 만약 버튼 클릭과 겹치는 게 걱정된다면 이 로그를 보고 UI를 치워야 합니다.
                    Debug.Log("UI가 감지되었지만 배치를 강행합니다.");
                }

                if (canBuild)
                {
                    BuildUnit(spawnPos); // 실제 배치 함수 호출
                }
            }

            if (Input.GetMouseButtonDown(1)) ClearSelection();
        }
    }

    bool CheckCanBuild(Vector3 pos)
    {
        if (deployedUnits.Count >= maxUnitCount) return false;

        // 유닛 콜라이더 잠시 끄기 (가림 방지)
        Collider2D previewCol = previewObject.GetComponent<Collider2D>();
        if (previewCol != null) previewCol.enabled = false;

        // 마우스 위치에 'Ground' 레이어가 있는지 확인
        Collider2D groundHit = Physics2D.OverlapPoint(pos, groundLayer);

        if (previewCol != null) previewCol.enabled = true;

        if (groundHit == null) return false;

        // 거리 체크
        foreach (GameObject unit in deployedUnits)
        {
            if (unit != null && Vector2.Distance(pos, unit.transform.position) < minDistanceBetweenUnits)
                return false;
        }

        return true;
    }

    public void SelectUnitToBuild(UnitData data)
    {
        if (data == null) return;
        if (previewObject != null) Destroy(previewObject);

        unitToBuild = data;
        previewObject = Instantiate(unitToBuild.towerPrefab);

        Tower t = previewObject.GetComponent<Tower>();
        if (t != null) t.isPlaced = false;

        SetUnitAlpha(previewObject, 0.5f);

        Transform circle = previewObject.transform.Find("Circle");
        if (circle != null)
        {
            previewRangeRenderer = circle.GetComponent<SpriteRenderer>();
            float range = data.stats[0].range;
            circle.localScale = new Vector3(range * 2, range * 2, 1);
            circle.gameObject.SetActive(true);
        }
    }

    void BuildUnit(Vector3 pos)
    {
        Debug.Log("클릭 감지됨! 설치를 시도합니다."); // 이 로그가 뜨는지 확인
        if (unitToBuild == null) return;

        // 1. 유닛 데이터가 없는지 확인
        if (unitToBuild == null)
        {
            Debug.LogError("설치할 유닛 데이터가 선택되지 않았습니다!");
            return;
        }

        // 2. [가장 의심되는 곳] 돈이 부족한지 확인
        if (PlayerStats.Money < unitToBuild.unitPrice)
        {
            Debug.LogWarning("돈이 부족합니다! 현재 잔액: " + PlayerStats.Money + " / 필요 금액: " + unitToBuild.unitPrice);
            return;
        }

        // 3. 실제 생성
        Debug.Log("유닛 생성 시도: " + unitToBuild.unitName);
        GameObject finalUnit = Instantiate(unitToBuild.towerPrefab, pos, Quaternion.identity);

        Tower t = finalUnit.GetComponent<Tower>();
        if (t != null)
        {
            t.data = unitToBuild;
            t.isPlaced = true;
        }

        deployedUnits.Add(finalUnit);
        PlayerStats.Money -= unitToBuild.unitPrice;

        Debug.Log("설치 완료! 남은 돈: " + PlayerStats.Money);
        ClearSelection();
    }

    public void ClearSelection()
    {
        unitToBuild = null;
        if (previewObject != null) Destroy(previewObject);
        previewRangeRenderer = null;
    }

    void SetUnitAlpha(GameObject obj, float alphaValue)
    {
        foreach (var r in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            if (r.gameObject.name == "Circle") continue;
            Color c = r.color; c.a = alphaValue; r.color = c;
        }
    }
}