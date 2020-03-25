using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : MonoBehaviour
{
     // For targeting enemy
    private List<GameObject> target;
    private Virus virus;
    private Animator anim;
    
    [Header("Attributes")]
    public float range = 15f;
    public int  damage = 20;
    public float slowAmount = 0.3f;
   
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public GameObject projectile;
    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        EventManager.StartListening("HeartBeat", HandleHeartBeat);
        target = new List<GameObject>();
    }

    void UpdateTarget () 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        target.Clear();

        foreach(GameObject enemy in enemies)
        {
            // Could have to be vector3
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < range)
            {
                target.Add(enemy);
            }
        }
    }

    void HandleHeartBeat()
    {
        UpdateTarget();
        anim.SetTrigger("Beat");
        if(target == null) { return; }
        Shoot();
    }

    void Shoot()
    {
        foreach(GameObject enemy in target)
        {
            Virus virus = enemy.GetComponent<Virus>();
            virus.Slow(slowAmount);
        }
        
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);    
    }
}
