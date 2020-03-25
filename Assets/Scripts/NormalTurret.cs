using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTurret : MonoBehaviour
{
    Animator anim;
    // For targeting enemy
    private Transform target;
    [Header("Attributes")]
    public float range = 15f;
    public int  damage = 20;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
        EventManager.StartListening("HeartBeat", HandleHeartBeat);
    }

    void UpdateTarget () 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            // Could have to be vector3
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else 
        {
            target = null;
        }
    }

    void HandleHeartBeat()
    {
        if(target == null) { return; }
        anim.SetTrigger("Beat");
        Shoot();
    }

    void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, transform.rotation).gameObject;
        Projectile proj = projectileGO.GetComponent<Projectile>();

        if(proj != null)
        {
            proj.Seek(target);
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);    
    }
}
