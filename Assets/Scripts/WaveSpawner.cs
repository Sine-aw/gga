using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 리스트 사용을 위해 추가

[System.Serializable] // 유니티 인스펙터에 보이게 함
public struct WaveContent
{
    public EnemyData enemyData; // 어떤 적을
    public int count;           // 몇 마리 소환할지
}

[System.Serializable]
public struct Wave
{
    public List<WaveContent> enemyList; // 한 웨이브에 여러 종류의 적 구성 가능
    public float rate;                  // 소환 간격
}

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public List<Wave> waves; // 인스펙터에서 웨이브를 여러 개 만들 수 있습니다.

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    private int waveIndex = 0;

    void Start()
    {
        // 씬이 시작될 때 시간을 정상 속도(1배속)로 강제 고정
        Time.timeScale = 1f;

        // 기존 설정들
        countdown = 2f;
        waveIndex = 0;
    }

    void Update()
    {
        // 1. 모든 웨이브 소환이 끝났는지 확인
        if (waveIndex >= waves.Count)
        {
            // 2. 필드에 남은 적이 한 마리도 없는지 확인
            // Enemy 스크립트가 붙은 오브젝트가 0개라면 클리어!
            if (GameObject.FindObjectsOfType<Enemy>().Length == 0)
            {
                // 중복 실행 방지 (이미 패널이 떠 있다면 패스)
                if (ResultUI.instance != null && !ResultUI.instance.resultPanel.activeSelf)
                {
                    Debug.Log("모든 웨이브 클리어! 보상 지급 및 패널 오픈");
                    GameResultManager.instance.MissionComplete();
                }
            }
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[waveIndex];

        foreach (WaveContent content in currentWave.enemyList)
        {
            for (int i = 0; i < content.count; i++)
            {
                SpawnEnemy(content.enemyData);
                yield return new WaitForSeconds(currentWave.rate);
            }
        }

        // [웨이브 클리어 보상 추가] 웨이브 하나가 끝날 때마다 골드 지급
        if (GameResultManager.instance != null)
        {
            GameResultManager.instance.AddWaveReward();
        }

        waveIndex++;
    }

    void SpawnEnemy(EnemyData data)
    {
        Vector3 pos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        GameObject enemyGO = Instantiate(enemyPrefab, pos, Quaternion.identity);

        Enemy enemy = enemyGO.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Setup(data, Waypoints.points);
        }
    }
}