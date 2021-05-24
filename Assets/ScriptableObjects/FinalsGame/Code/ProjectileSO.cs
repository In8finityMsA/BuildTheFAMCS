using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Projectile", menuName = "SO/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public GameObject projectilePrefab;
    public Sprite projectileSprite;

    public float speed;
    public float rotationSpeed;
}
