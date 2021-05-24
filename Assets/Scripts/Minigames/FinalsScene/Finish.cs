using System;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public event Action<GameObject> OnEnemyFinished;

    private void OnTriggerEnter2D(Collider2D other1)
    {
        OnEnemyFinished?.Invoke(other1.gameObject);
        Destroy(other1.gameObject);
    }
}
