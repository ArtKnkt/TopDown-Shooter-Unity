using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 10;
    [SerializeField] int shieldHealth;

    [Header("Shooting")]
     float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float durationOfExplosion = 1f;
   

    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject shieldVFX;

    [SerializeField] float durationOfPing = .2f;
    [SerializeField] AudioClip enemyFireSFX;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range (0,1)] float shootVolume = .5f;
    [SerializeField] [Range(0, 1)] float dieVolume = .5f;
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        CountDownAndShoot();
   
    }
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
           GameObject laser = Instantiate(
                 enemyProjectile, transform.position, Quaternion.identity)
                  as GameObject;

        AudioSource.PlayClipAtPoint(enemyFireSFX, Camera.main.transform.position, shootVolume);

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ShieldDmgDealer shieldDmgDealer = other.gameObject.GetComponent<ShieldDmgDealer>();
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        
     // if (!damageDealer) { return; }
        
       if (shieldHealth > 0)
            ProcessShieldHit(shieldDmgDealer);

        if (shieldHealth <= 0)
            ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
 
        if (health <= 0)
        {
            Die();
        }
    }
    private void ProcessShieldHit(ShieldDmgDealer shieldDmgDealer)
    {
        shieldHealth -= shieldDmgDealer.GetShieldDamage();
        shieldDmgDealer.Hit();

        GameObject shieldPing = Instantiate(shieldVFX, transform.position,
            Quaternion.Euler(180, 0, 0));

        Destroy(shieldPing, durationOfPing);
    }
    
    private void Die()
    {
     FindObjectOfType<GameSession>().AddToScore(scoreValue);
        ;
     Destroy(gameObject);
     AudioSource.PlayClipAtPoint(deathSFX, FindObjectOfType<Camera>().transform.position, dieVolume);
     GameObject explosion = Instantiate(deathVFX, transform.position,
        transform.rotation);
     Destroy(explosion, durationOfExplosion);

    }

    public float GetHealth()
    {
        return health;
    }
}