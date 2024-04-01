using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageManager : MonoBehaviour
{

    public PostProcessVolume _volume;
    public float bloodIntensity;
    Vignette _vignette;

    void Start()
    {
        _volume.profile.TryGetSettings(out _vignette);

        if(!_vignette){
            Debug.LogError("No vignette found");
        }
        else{
            _vignette.enabled.Override(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyProjectile")
        {
            Debug.Log("PLAYER HIT");
            transform.parent.GetComponent<PlayerScript>().Damage(10);
            StartCoroutine(DamageEffect());
            Destroy(other.gameObject);
        }
    }


    private IEnumerator DamageEffect(){
        //isDamageEffectRunning = true;
        _vignette.enabled.Override(false);
        _vignette.intensity.Override(0f);
        bloodIntensity = .4f;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(bloodIntensity);

        yield return new WaitForSeconds(0.4f);

        while (bloodIntensity > 0)
        {
            bloodIntensity -= Time.deltaTime / 2;
            _vignette.intensity.Override(bloodIntensity);
            yield return null;
        }
        _vignette.intensity.Override(0f);
        _vignette.enabled.Override(false);
        //isDamageEffectRunning = false;
    }
}
