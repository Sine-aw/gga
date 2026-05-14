using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float hp;
    public float speed;
    public Sprite enemySprite;
    public int goldReward;
}