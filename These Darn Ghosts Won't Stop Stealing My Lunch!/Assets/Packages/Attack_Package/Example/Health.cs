using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    public int currentHealth;
    float nhealth;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private AudioClip hitSound;

    public void hurt(int damage)
    {
        currentHealth -= damage;
        if(hitSound)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    private void Start()
    {
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        nhealth = currentHealth;
    }

    private void Update()
    {
        if (Mathf.RoundToInt(nhealth) != currentHealth)
        {
            nhealth = Mathf.Lerp(nhealth, currentHealth, 6*Time.deltaTime);
            if (healthBar)
                healthBar.value = nhealth;
        }
        if(currentHealth <= 0)
        {
            die();
        }
        //nhealth = currentHealth;
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
