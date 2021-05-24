using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "SO/Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject enemyPrefab;
    
    public float health;
    public float speed;
    public int damage;
    public int coinsOnDieLowerBound;
    public int coinsOnDieUpperBound;
}
