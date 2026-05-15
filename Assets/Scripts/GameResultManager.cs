using UnityEngine;

public class GameResultManager : MonoBehaviour
{
    public static GameResultManager instance;
    public int waveReward = 100; // 웨이브당 골드
    public int finalBonus = 1000; // 최종 클리어 보너스

    void Awake() => instance = this;

    // 매 웨이브 끝날 때 호출
    public void AddWaveReward()
    {
        int currentGold = PlayerPrefs.GetInt("TotalGold", 0);
        PlayerPrefs.SetInt("TotalGold", currentGold + waveReward);
        PlayerPrefs.Save();
    }

    // 마지막 클리어 시 호출
    public void MissionComplete()
    {
        int currentGold = PlayerPrefs.GetInt("TotalGold", 0);
        PlayerPrefs.SetInt("TotalGold", currentGold + finalBonus);
        PlayerPrefs.Save();

        if (ResultUI.instance != null)
        {
            ResultUI.instance.ShowResult(true, finalBonus);
        }
    }
}