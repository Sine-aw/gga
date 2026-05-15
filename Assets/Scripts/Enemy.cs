using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float health = 50f;
    private Transform[] points;
    private int destPoint = 0;
    private int myGoldReward;
    private float maxHealth;

    [Header("UI 연결")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    void Start()
    {
        // 만약 인스펙터에서 health를 100으로 설정했다면, 그 값을 최대 체력으로 삼습니다.
        if (health > 0)
        {
            maxHealth = health;
        }
        else
        {
            health = 100f; // 기본값 방어 코드
            maxHealth = 100f;
        }

        UpdateHealthUI();
    }

    public void Setup(EnemyData data, Transform[] waypoints)
    {
        this.points = waypoints;
        this.speed = data.speed;
        this.health = data.hp; // EnemyData.hp와 일치
        this.myGoldReward = data.goldReward;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        if (sr != null && data != null)
        {
            sr.sprite = data.enemySprite;
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.sortingOrder = 20;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount; // 데미지 적용
        UpdateHealthUI(); // UI 갱신

        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            // 0~1 사이의 비율을 정확히 계산 (최소 0은 보장)
            healthSlider.value = Mathf.Clamp01(health / maxHealth);
        }

        if (healthText != null)
        {
            // 소수점을 제외하고 정수만 깔끔하게 나오도록 수정
            // 예: "100 / 100"
            healthText.text = string.Format("{0} / {1}", Mathf.CeilToInt(health), Mathf.CeilToInt(maxHealth));
        }
    }

    public float GetDistanceToGoal()
    {
        return (points.Length - destPoint) + Vector2.Distance(transform.position, points[destPoint].position);
    }

    void Die()
    {
        PlayerStats.Money += myGoldReward; // PlayerStats.Money와 연결
        Destroy(gameObject);
    }

    // 수정된 코드 (Enemy.cs)
    void Update()
    {
        if (points == null || destPoint >= points.Length) return;

        Vector3 target = points[destPoint].position;
        Vector3 moveDir = new Vector3(target.x, target.y, transform.position.z) - transform.position;

        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target.x, target.y)) < 0.1f)
        {
            destPoint++;

            // 마지막 웨이포인트(성 위치)에 도달했는지 체크
            if (destPoint >= points.Length)
            {
                ReachGoal(); // 여기서 데미지를 입히고 파괴하도록 변경
                return;      // 더 이상 아래 로직을 실행하지 않도록 리턴
            }
        }
    }

    // Enemy.cs 내의 도달 처리 부분
    void ReachGoal()
    {
        if (CastleStats.instance != null)
        {
            // 1. 먼저 데미지를 입힘 (정수 형변환 확인)
            CastleStats.instance.TakeDamage((int)this.health);
            Debug.Log("성 공격 성공! 남은 HP: " + CastleStats.instance.health);
        }
        else
        {
            // 만약 이 로그가 뜬다면 CastleStats 오브젝트가 씬에 없거나 instance 연결이 안 된 것임
            Debug.LogError("CastleStats 인스턴스를 찾을 수 없습니다!");
        }

        // 2. 그 다음 적을 파괴
        Destroy(gameObject);
    }
}