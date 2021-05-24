using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public ProjectileSO projectileSettings = null;
    [HideInInspector] public GameObject target = null;
    [HideInInspector] public float damage;

    private bool _TargetSetted = false;

    public void Initialize(GameObject obj, float towerDamage, ProjectileSO settings)
    {
        projectileSettings = settings;
        GetComponent<SpriteRenderer>().sprite = projectileSettings.projectileSprite;
        target = obj;
        damage = towerDamage;
        SetInitialRotation();
        _TargetSetted = true;
    }

    private void Update()
    {
        if (_TargetSetted == false)
            return;
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        MoveTowards(dir);
        RotateTowards(dir);
        
        if (Vector3.Distance(transform.position, target.transform.position) <= 0.15f)
        {
            target.GetComponent<Enemy>().ApplyDamage(damage);
            Destroy(gameObject);
        }
    }

    private void MoveTowards(Vector3 dir)
    {
        transform.Translate(dir.normalized * projectileSettings.speed * Time.deltaTime, Space.World);
    }

    private void RotateTowards(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, projectileSettings.rotationSpeed * Time.deltaTime);
    }

    private void SetInitialRotation()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
