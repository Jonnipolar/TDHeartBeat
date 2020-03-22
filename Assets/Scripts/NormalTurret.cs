using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTurret : MonoBehaviour
{
    // For targeting enemy
    private Transform target;
    [Header("Attributes")]
    public float range = 15f;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public GameObject projectile;
    // Might not need
    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
    //     if(target == null) { return; }

    //     if(fireCountdown <= 0f)
    //     {
    //         Shoot();
    //         fireCountdown = 1f / fireRate;
    //     }

    //     fireCountdown -= Time.deltaTime;
    }

    void HandleHeartBeat()
    {
        if(target == null) { return; }
        Shoot();
    }

    void Shoot()
    {
        GameObject projectileGO = (GameObject) Instantiate(projectile, firePoint.position, firePoint.rotation);
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
