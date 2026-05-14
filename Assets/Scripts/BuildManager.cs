using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private UnitData unitToBuild;
    private GameObject previewObject;

    public int maxUnitCount = 12;
    private List<GameObject> deployedUnits = new List<GameObject>();

    void Awake() { instance = this; }

    void Update()
    {
        if (unitToBuild != null)
        {
            MovePreview();

            // --- [실시간 설치 가능 여부 체크 추가] ---
            CheckPlacementValidity();
            // --------------------------------------

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 clickPos2D = new Vector2(mPos.x, mPos.y);
                Collider2D groundHit = Physics2D.OverlapPoint(clickPos2D);

                Collider2D overlapHit = Physics2D.OverlapCircle(clickPos2D, 0.4f, LayerMask.GetMask("Tower"));

                if (groundHit != null && groundHit.CompareTag("Ground") && overlapHit == null)
                {
                    // 3. [추가] 인원수 제한 확인
                    if (deployedUnits.Count < maxUnitCount)
                    {
                        Vector3 buildPos = new Vector3(mPos.x, mPos.y, 0f);
                        BuildTower(unitToBuild.towerPrefab, unitToBuild.stats[0].upgradeCost, buildPos);
                    }
                    else
                    {
                        Debug.Log("최대 유닛 설치 개수(12마리)를 초과했습니다!");
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) ClearSelection();
        }
    }

    // 실시간으로 바닥 태그를 확인해서 색상을 바꾸는 함수
    void CheckPlacementValidity()
    {
        if (previewObject == null) return;

        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mPos);

        // 사거리 원(Circle) 오브젝트를 찾습니다.
        Transform rangeCircle = previewObject.transform.Find("Circle");
        if (rangeCircle == null && previewObject.transform.childCount > 0)
            rangeCircle = previewObject.transform.GetChild(0);

        if (rangeCircle != null)
        {
            SpriteRenderer sr = rangeCircle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // 태그가 "Ground"면 하얀색(또는 원래색), 아니면 빨간색
                if (hit != null && hit.CompareTag("Ground"))
                {
                    sr.color = new Color(1f, 1f, 1f, 0.3f); // 정상 (반투명 흰색)
                }
                else
                {
                    sr.color = new Color(1f, 0f, 0f, 0.4f); // 설치 불가 (반투명 빨간색)
                }
            }
        }
    }

    public void SelectUnitToBuild(UnitData unit)
    {
        unitToBuild = unit;
        if (previewObject != null) Destroy(previewObject);

        if (unit.unitModel != null)
        {
            previewObject = Instantiate(unit.unitModel);
            SetUnitAlpha(previewObject, 0.5f);

            // 미리보기 유닛의 공격 기능 끄기
            Tower t = previewObject.GetComponent<Tower>();
            if (t != null) t.enabled = false;

            foreach (var col in previewObject.GetComponentsInChildren<Collider2D>())
                col.enabled = false;

            // 사거리 원 설정
            UpdateRangeVisual(unit.stats[0].range);
        }
    }

    void BuildTower(GameObject turretPrefab, int cost, Vector3 position)
    {
        if (PlayerStats.Money < cost) return;

        PlayerStats.Money -= cost;
        if (PlayerStats.instance != null) PlayerStats.instance.UpdateMoneyUI();

        GameObject newUnit = Instantiate(turretPrefab, position, Quaternion.identity);
        deployedUnits.Add(newUnit);

        // [중요] 설치가 끝났으므로 미리보기를 삭제하고 선택을 해제합니다.
        ClearSelection();
    }

    public void RemoveUnit(GameObject unit)
    {
        if (deployedUnits.Contains(unit))
        {
            deployedUnits.Remove(unit);
        }
    }

    // 마우스를 따라오게 하는 핵심 함수
    void MovePreview()
    {
        if (previewObject == null) return;

        // 이름을 겹치지 않게 'pPos'로 수정
        Vector3 pPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previewObject.transform.position = new Vector3(pPos.x, pPos.y, 0f);
    }

    void UpdateRangeVisual(float range)
    {
        Transform rangeCircle = previewObject.transform.Find("Circle");
        if (rangeCircle == null && previewObject.transform.childCount > 0)
            rangeCircle = previewObject.transform.GetChild(0);

        if (rangeCircle != null)
        {
            rangeCircle.gameObject.SetActive(true);
            float s = range * 2f;
            rangeCircle.localScale = new Vector3(s, s, 1f);
        }
    }

    void SetUnitAlpha(GameObject obj, float alphaValue)
    {
        foreach (var r in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = r.color;
            c.a = (r.gameObject.name == "Circle") ? 0.3f : alphaValue;
            r.color = c;
        }
    }

    void ClearSelection()
    {
        unitToBuild = null;
        if (previewObject != null) Destroy(previewObject); // 미리보기 파괴
    }

    public void SelectTowerToBuild(TowerData tower)
    {
        
    }
}