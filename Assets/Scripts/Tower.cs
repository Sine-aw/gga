using UnityEngine;

public class Tower : MonoBehaviour
{
    public UnitData data;
    public Transform firePoint;
    public int currentLevel = 1;

    private Transform target;
    private float fireCountdown = 0f;

    void Update()
    {
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

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            // 1. 사거리 안에 있는지 확인
            if (distanceToEnemy <= data.stats[currentLevel - 1].range)
            {
                // 2. 적의 Enemy 스크립트에서 남은 거리 정보를 가져옴
                // (Enemy.cs에 Waypoint 인덱스나 이동 거리를 계산하는 변수가 있다고 가정)
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // 적이 골인 지점까지 남은 거리가 지금까지 찾은 적보다 짧다면 타겟 변경
                    // 만약 Enemy.cs에 남은 거리 변수가 없다면 아래 '팁' 참고
                    float distanceToGoal = enemyScript.GetDistanceToGoal();

                    if (distanceToGoal < shortestDistanceToGoal)
                    {
                        shortestDistanceToGoal = distanceToGoal;
                        nearestEnemy = enemy;
                    }
                }
            }
        }

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
}
