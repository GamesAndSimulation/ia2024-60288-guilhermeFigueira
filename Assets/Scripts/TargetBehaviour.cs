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
    public AudioClip _DieSound;
    private Vector3 _firstOriginalScale;
 
 
    private Vector3 direction;
    private bool isDying;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeDirection", 0, Random.Range(directionTime/1.5f, directionTime));
        _firstOriginalScale = transform.localScale;
        isDying = false;
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
        instantiatedBullet.GetComponent<SphereCollider>().isTrigger = true;
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(fireDirection * fireforce);
        Destroy(instantiatedBullet, 5);
    }
 
    // Update is called once per frame
    void Update()
    {
        transform.position += direction;
        transform.LookAt(Camera.main.transform); //Billboard effect
    }
 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile" && !isDying)
        {
            if(heath > 0){
                heath -= 10;
                StartCoroutine(Squish());
                if (heath <= 0)
                {
                    AudioManager.Instance.PlaySound(_DieSound, true);
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
                    AudioManager.Instance.PlaySound(_DieSound, true);
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
        Vector3 stretchedScale = new Vector3(originalScale.x * 1.5f, originalScale.y * 0.6f, originalScale.z);
        Vector3 squishedScale = new Vector3(originalScale.x * 0.6f, originalScale.y * 1.5f, originalScale.z);
 
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