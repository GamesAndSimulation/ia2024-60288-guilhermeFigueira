using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JetpackManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem jetpackSmoke;
    [SerializeField] private AudioClip jetpackSound;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private float fullFuel = 2.0f;
    private float fuel;
    private MainScript player;

    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<MainScript>();
        fuel = fullFuel;
        fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
    }

    void Update(){
        if(Input.GetButtonDown("Jump")){
            jetpackSmoke.Play();
            AudioManager.Instance.PlaySoundLooping(jetpackSound);
        }

        if(Input.GetButton("Jump"))
        {
            if(fuel > 0)
            {
                fuel -= Time.deltaTime;
                fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
                player.SetJetpackForce();
                // Add force to the player
            }
            else
            {
                jetpackSmoke.Stop();
                AudioManager.Instance.StopSoundLooping();
            }
        }
 
        if(Input.GetButtonUp("Jump")) {
            jetpackSmoke.Stop();
            AudioManager.Instance.StopSoundLooping();
        }
        if(player.isGrounded){
            fuel = fullFuel;
            fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
        }
    }
}
