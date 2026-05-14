using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int price;
    public Sprite towerIcon;
    public GameObject towerPrefab; // 실제 설치될 타워 프리팹
}