using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JetpackManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem jetpackSmoke;
    [SerializeField] private AudioClip jetpackSound;
    public TextMeshProUGUI fuelText;
    public float fullFuel = 2.0f;
    public float flyForce = 0.2f;
    public float fuel;
    private PlayerScript player;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private GameObject playerOrientation;
    public bool usingJetpack { get; private set; } = false;

    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        fuel = fullFuel;
        fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
        usingJetpack = false;
    }

    void Update()
    {
        jetpackSmoke.transform.forward = -playerOrientation.transform.right;
        if(Input.GetButtonDown("Jump") && !player.grounded && !usingJetpack){
            jetpackSmoke.Play();
            AudioManager.Instance.PlaySoundLooping(jetpackSound);
            if(fuel > 0)
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
            usingJetpack = true;
        }

        if(usingJetpack)
        {
            if(fuel > 0)
            {
                fuel -= Time.deltaTime * 0.8f;
                fuelText.text = $"{fuel.ToString("F1")} / {fullFuel.ToString("F1")}";
                float jetpackVelocityY = 5f;
                playerRB.velocity = new Vector3(playerRB.velocity.x, jetpackVelocityY, playerRB.velocity.z);
                //playerRB.AddForce(Vector3.up * flyForce, ForceMode.Force);
                //player.UpwardsForce(flyForce);
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
