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

    void Update()
    {
        if (waveIndex >= waves.Count) return; // 모든 웨이브 종료 시 중단

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[waveIndex]; // 현재 웨이브 설정 가져오기

        // 웨이브에 설정된 적 종류만큼 반복
        foreach (WaveContent content in currentWave.enemyList)
        {
            // 설정된 마릿수만큼 반복 소환
            for (int i = 0; i < content.count; i++)
            {
                SpawnEnemy(content.enemyData);
                yield return new WaitForSeconds(currentWave.rate);
            }
        }

        waveIndex++; // 다음 웨이브로
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