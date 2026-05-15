using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public float range = 5f;
    public Transform firePoint;
    public int currentLevel = 1;

    private Transform target;
    private float fireCountdown = 0f;

    public bool isPlaced = false;

    public void OnUnitClicked()
    {
        // 배치 중일 때는 정보창 열기 차단
        if (BuildManager.instance != null && BuildManager.instance.IsBuildingMode()) return;

        if (UnitUpgradeUI.instance != null)
        {
            UnitUpgradeUI.instance.SetTarget(this);
        }
    }

    void Update()
    {
        if (!isPlaced) return;

        UpdateTarget();

        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / data.stats[currentLevel - 1].fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistanceToGoal = Mathf.Infinity; // 남은 거리가 가장 짧은 적을 찾기 위함
        GameObject nearestEnemy = null;

        foreach (GameObject enemyGO in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyGO.transform.position);

            // 1. 일단 내 사거리 안에 있는지 확인
            if (distanceToEnemy <= range)
            {
                Enemy enemyScript = enemyGO.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // 2. 성까지 남은 거리를 가져옴
                    float distanceToGoal = enemyScript.GetDistanceToGoal();

                    // 3. 현재 타겟보다 더 성에 가까이 간(남은 거리가 짧은) 적을 발견하면 교체
                    if (distanceToGoal < shortestDistanceToGoal)
                    {
                        shortestDistanceToGoal = distanceToGoal;
                        nearestEnemy = enemyGO;
                    }
                }
            }
        }

        // 최종적으로 가장 앞선 적을 타겟으로 설정
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        // 1. 투사체 프리팹이 있는지 확인 (UnitData에서 설정함)
        if (data.projectilePrefab != null && target != null)
        {
            // 2. 발사 위치(firePoint)에서 투사체 생성
            GameObject bulletGO = Instantiate(data.projectilePrefab, firePoint.position, firePoint.rotation);

            // 3. 생성된 투사체에 타겟과 데미지 정보 전달
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                // UnitLevelStat 배열에서 현재 레벨의 데미지를 가져옴
                bullet.damage = data.stats[currentLevel - 1].damage;
                bullet.Seek(target); // Bullet.cs의 Seek 함수 호출
            }
        }
    }

    // Tower.cs에 추가
    // Tower.cs에 추가
    // Tower.cs에 추가
    void OnMouseDown()
    {
        if (!isPlaced) return;

        // 1. 빌드 모드일 때는 차단 (기존 로직 유지)
        if (BuildManager.instance != null && BuildManager.instance.IsBuildingMode()) return;

        // 2. [수정] UI 체크 로그는 남기되, 클릭은 진행하도록 return을 주석처리합니다.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI가 가리고 있지만, 무시하고 정보창을 엽니다.");
            // return; // 이 부분을 주석 처리하면 UI 뒤에 있는 유닛도 클릭됩니다!
        }

        // 3. 정보창 활성화
        if (UnitUpgradeUI.instance != null)
        {
            Debug.Log(data.unitName + " 정보창 오픈 시도");
            UnitUpgradeUI.instance.SetTarget(this);
        }
    }

    // Tower.cs 내부에 추가
    public void ShowRange(bool show)
    {
        Transform circle = transform.Find("Circle");
        if (circle != null)
        {
            circle.gameObject.SetActive(show);
            if (show)
            {
                float range = data.stats[currentLevel - 1].range;
                circle.localScale = new Vector3(range * 2f, range * 2f, 1f);
            }
        }
    }
}
