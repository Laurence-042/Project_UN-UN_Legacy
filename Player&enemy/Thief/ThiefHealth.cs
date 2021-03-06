﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ThiefHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    ThiefMovement playerMovement;                              // Reference to the player's movement.
    ThiefFight thiefFight;                              // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.


    void Awake()
    {
        // Setting up the references.
        anim = GetComponentInChildren<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<ThiefMovement>();
        thiefFight = GetComponentInChildren<ThiefFight>();

        if (healthSlider == null)
        {
            healthSlider = GameObject.FindGameObjectWithTag("HPbar").GetComponent<Slider>();
        }
        if (damageImage == null)
        {
            damageImage = GameObject.FindGameObjectWithTag("DamageEffect").GetComponent<Image>();
        }
        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;

        if (isDead)
        {
            CharacterController controller = GetComponent<CharacterController>();
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            controller.Move(-Vector3.up);
        }
    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = (float)currentHealth / 100f;

        // Play the hurt sound effect.
        //playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;


        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        thiefFight.enabled = false;
        playerAudio.enabled = false;
        StartCoroutine(ClearBody());
    }

    public void RestartLevel()
    {
        // Reload the level that is currently loaded.
        SceneManager.LoadScene(0);
    }
    IEnumerator ClearBody()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }
}
