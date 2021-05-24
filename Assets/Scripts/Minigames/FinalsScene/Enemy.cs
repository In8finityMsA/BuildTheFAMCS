using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO enemySettings = null;
    [SerializeField] public Image healthBar = null;

    private float _Speed;
    private int _WaypointIndex = 0;
    private Transform _Target;

    public float Health { get; private set; }
    public int Damage { get; private set; }
    public int CoinsOnDie { get; private set; }

    public event Action<Enemy> OnDied;
    void Start()
    {
        Health = enemySettings.health;
        _Speed = enemySettings.speed;
        Damage = enemySettings.damage;
        CoinsOnDie = Random.Range(enemySettings.coinsOnDieLowerBound, enemySettings.coinsOnDieUpperBound);

        healthBar.fillAmount = 1;
        
        _Target = Waypoints._Points[0];
    }

    void Update()
    {
        Vector3 dir = _Target.position - transform.position;
        transform.Translate(dir.normalized * _Speed * Time.deltaTime, Space.World);
        
        if (Vector3.Distance(transform.position, _Target.position) <= 0.15f)
        {
            GetNextWaypoint();
        }
    }

    public void ApplyDamage(float dmg)
    {
        Health -= dmg;
        healthBar.fillAmount = Health / enemySettings.health;
        if (Health <= 0)
        {
            OnDied?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void GetNextWaypoint()
    {
        if (_WaypointIndex >= Waypoints._Points.Length - 1)
            return;
        
        _WaypointIndex++;
        _Target = Waypoints._Points[_WaypointIndex];
    }
}
