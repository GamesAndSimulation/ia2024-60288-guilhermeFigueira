using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageManager : MonoBehaviour
{

    public PostProcessVolume _volume;
    public float bloodIntensity;
    Vignette _vignette;
    
    [SerializeField] private List<HostileThing> _hostileThings;
    private Dictionary<string, HostileThing> _hostileTagsAndDamage;
    
    public float continuousDamageDelay = 1f;
    private float continuousDamageDelayTimer;

    void Start()
    {
        continuousDamageDelayTimer = -1;
        _volume.profile.TryGetSettings(out _vignette);

        if(!_vignette){
            Debug.LogError("No vignette found");
        }
        else{
            _vignette.enabled.Override(false);
        }
        
        _hostileTagsAndDamage = new Dictionary<string, HostileThing>();

        foreach (HostileThing hostileThing in _hostileThings)
        {
            _hostileTagsAndDamage.Add(hostileThing.tag, hostileThing);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (_hostileTagsAndDamage.ContainsKey(other.tag))
        {
            if (!_hostileTagsAndDamage[other.tag].isContinuous)
            {
                transform.parent.GetComponent<PlayerScript>().ChangeHealth(_hostileTagsAndDamage[other.tag].damage);
                StartCoroutine(DamageEffect());
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_hostileTagsAndDamage.ContainsKey(other.tag) && _hostileTagsAndDamage[other.tag].isContinuous)
        {
            if(continuousDamageDelayTimer > 0){
                continuousDamageDelayTimer -= Time.deltaTime;
                return;
            } 
            
            continuousDamageDelayTimer = continuousDamageDelay;
            transform.parent.GetComponent<PlayerScript>().ChangeHealth(_hostileTagsAndDamage[other.tag].damage);
            StartCoroutine(DamageEffect());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_hostileTagsAndDamage.ContainsKey(other.tag) && _hostileTagsAndDamage[other.tag].isContinuous)
        {
            continuousDamageDelayTimer = -1;
        }
    }
    
    private IEnumerator DamageEffect(){
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
    }
}

[Serializable]
public class HostileThing
{
    public string tag;
    public int damage;
    public bool isContinuous;
}
