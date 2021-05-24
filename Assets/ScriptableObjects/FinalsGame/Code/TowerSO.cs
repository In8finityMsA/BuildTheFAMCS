using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "SO/Tower")]
public class TowerSO : ScriptableObject
{
    public GameObject towerPrefab;

    public ProjectileSO projectile;

    public float shootInterval;
    public float range;
    public float damage;

    public int buildPrice;
}
