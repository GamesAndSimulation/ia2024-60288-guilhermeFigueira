using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JetpackManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem jetpackSmoke;
    [SerializeField] private AudioClip jetpackSound;
    [SerializeField] private TextMeshProUGUI fuelText;
    public float fullFuel = 2.0f;
    public float flyForce = 0.2f;
    private float fuel;
    private PlayerScript player;
    public bool usingJetpack { get; private set; } = false;

    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        fuel = fullFuel;
        fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
        usingJetpack = false;
    }

    void Update(){
        if(Input.GetButtonDown("Jump") && !player.grounded && !usingJetpack){
            jetpackSmoke.Play();
            AudioManager.Instance.PlaySoundLooping(jetpackSound);
            usingJetpack = true;
        }

        if(usingJetpack)
        {
            if(fuel > 0)
            {
                fuel -= Time.deltaTime * 0.8f;
                fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
                player.UpwardsForce(flyForce);
            }
            else
            {
                jetpackSmoke.Stop();
                AudioManager.Instance.StopSoundLooping();
            }
        }
 
        if(Input.GetButtonUp("Jump")) {
            usingJetpack = false;
            jetpackSmoke.Stop();
            AudioManager.Instance.StopSoundLooping();
        }
        if(player.grounded){
            fuel = fullFuel;
            fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
        }
    }
}
