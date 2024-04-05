using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TargetBehaviour : MonoBehaviour
{
    public GameObject projectile;
    public float fireforce;
    public float speedRange;
    public float directionTime;
    public float heath = 10;
 
    public float _squishDuration = 0.8f;
    public float stretched = 1.5f;
    public float squished = 0.6f;
    public AudioClip _DieSound;
    private Vector3 _firstOriginalScale;
 
 
    private Vector3 direction;
    private bool isDying;

    private bool _isFinalArenaEnemy;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeDirection", 0, Random.Range(directionTime/1.5f, directionTime));
        _firstOriginalScale = transform.localScale;
        isDying = false;
        var finalDoor = GameObject.FindWithTag("FinalDoor");
        _isFinalArenaEnemy = false;
        if (transform.position.z > finalDoor.transform.position.z)
        {
             _isFinalArenaEnemy = true;
             finalDoor.GetComponent<FinalArenaTrigger>().AddEnemyToKill();
             Debug.Log($"Final enemies: {finalDoor.GetComponent<FinalArenaTrigger>().enemiesToKill}");
        }
    }
 
    void ChangeDirection()
    {
        if(isDying) return;
        direction = new Vector3(Random.Range(-speedRange, speedRange), 0, Random.Range(-speedRange, speedRange));
        FireProjectile();
    }
 
    void FireProjectile()
    {
        if(isDying) return;
        var player = GameObject.FindGameObjectWithTag("Player");
        var fireDirection = player.transform.position - transform.position;
        fireDirection.Normalize();
        GameObject instantiatedBullet =
            Instantiate(projectile, transform.position + fireDirection, transform.rotation);
        instantiatedBullet.tag = "EnemyProjectile";
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(fireDirection * fireforce);
        Destroy(instantiatedBullet, 5);
    }
 
    public float MonsterSpeed;

    // Update is called once per frame
    void Update()
    {
        //transform.position += direction;
        GetComponent<Rigidbody>().AddForce(direction.normalized * MonsterSpeed, ForceMode.Force);
        transform.LookAt(Camera.main.transform); //Billboard effect
    }

    public GameObject explosion;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile" && !isDying)
        {
            if(heath > 0){
                Instantiate(explosion, collision.transform.position, Quaternion.identity);
                Instantiate(explosion, transform.position, Quaternion.identity);
                heath -= 10;
                StartCoroutine(Squish());
                if (heath <= 0)
                {
                    bool boss = heath >= 1000;

                    if (heath >= 1000)
                    {
                        AudioManager.Instance.PlayDeepSound(_DieSound);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(_DieSound, !boss);
                    }
                    GameObject.FindWithTag("Player").GetComponent<PlayerScript>().enemyKillCount += 1;
                    if (_isFinalArenaEnemy)
                        GameObject.FindWithTag("FinalDoor").GetComponent<FinalArenaTrigger>().KilledEnemy();
                    Destroy(gameObject);
                    isDying = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile" && !isDying)
        {
            if(heath > 0){
                heath -= 10;
                StartCoroutine(Squish());
                if (heath <= 0)
                {
                    if (transform.position.z > GameObject.FindWithTag("FinalDoor").transform.position.z)
                    {
                        GameObject.FindWithTag("FinalDoor").GetComponent<FinalArenaTrigger>().KilledEnemy();
                    }

                    AudioManager.Instance.PlaySound(_DieSound, true);
                    GameObject.FindWithTag("Player").GetComponent<PlayerScript>().enemyKillCount += 1;
                    Destroy(gameObject);
                    isDying = true;
                }
            }
        }
    }
 
    IEnumerator Squish()
    {
        float elapsed = 0;
 
        Vector3 originalScale = transform.localScale;
        Vector3 stretchedScale = new Vector3(originalScale.x * stretched, originalScale.y * squished, originalScale.z);
        Vector3 squishedScale = new Vector3(originalScale.x * squished, originalScale.y * stretched, originalScale.z);
 
        // Stretch
        while (elapsed < _squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _squishDuration
    ;
            transform.localScale = Vector3.Lerp(originalScale, stretchedScale, t);
            yield return null;
        }
 
        elapsed = 0;
 
        // Squish
        while (elapsed < _squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _squishDuration;
            transform.localScale = Vector3.Lerp(stretchedScale, squishedScale, t);
            yield return null;
        }
 
        elapsed = 0;
 
        // Return to original scale
        while (elapsed < _squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _squishDuration;
            transform.localScale = Vector3.Lerp(squishedScale, originalScale, t);
            yield return null;
        }
        transform.localScale = _firstOriginalScale;
    }
}