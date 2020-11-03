using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Object = UnityEngine.Object;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = -1;
    [SerializeField] int health;
    
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range (0,1)] float shootVolume = .5f;
    [SerializeField] [Range(0, 1)] float dieVolume = .5f;
    [SerializeField] AudioClip laserFire;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject shiledLaserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine; 
    Coroutine firingCoroutineAlt; 
    Coroutine firingCoroutineRight;

    Vector3 laserOffest = new Vector3(.55f,0);

     float xMin;
     float xMax;
     float yMin;
     float yMax;

    void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
        FireAlt();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
         if (!damageDealer) { return; }
            ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
       health -= damageDealer.GetDamage();
       damageDealer.Hit();
      if (health <= 0)
         Die();
     }
   
    private void Die()
    {
        health = 0;
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, dieVolume);
        FindObjectOfType<Level>().LoadGameOver();
    }
    public int GetHealth()
    {
        return health;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
       
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        //hard coded padding for maxY value below.
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, .75f, 0)).y - padding;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinouslyMain());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void FireAlt()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            firingCoroutineAlt = StartCoroutine(FireContinouslyAlt());
        }

        if (Input.GetButtonUp("Fire3"))
        {
            StopCoroutine(firingCoroutineAlt);
        }
    }

    public IEnumerator FireContinouslyMain()
    {
      while (true)
        {
            GameObject laser = Instantiate(
                        laserPrefab, transform.position, Quaternion.identity)
                    as GameObject;

                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserFire, Camera.main.transform.position, shootVolume);
        
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    public IEnumerator FireContinouslyAlt()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                    shiledLaserPrefab, transform.position - laserOffest, Quaternion.identity)
                as GameObject;

            GameObject laser2 = Instantiate(
                    shiledLaserPrefab, transform.position + laserOffest, Quaternion.identity)
                as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);

        }
    }
}  

