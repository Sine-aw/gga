using UnityEngine;

[System.Serializable]
public class UnitLevelStat
{
    public int level;
    public int damage;
    public float range;
    public float fireRate;
    public int upgradeCost;
}

[CreateAssetMenu(fileName = "UnitData", menuName = "TowerDefense/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int shopPrice;
    public GameObject towerPrefab;
    public GameObject unitModel;
    public GameObject projectilePrefab;
    public bool isUnlocked = false;

    public UnitLevelStat[] stats;

    public Sprite unitSprite;

    public void LoadStatus()
    {
        isUnlocked = PlayerPrefs.GetInt(unitName + "_Unlocked", 0) == 1;
    }
}