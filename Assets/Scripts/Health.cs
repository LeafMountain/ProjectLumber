using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public float percentage => health / maxHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(int value, Interactable instigator)
    {
        health += value;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(value < 0)
        {
            transform.DOPunchScale(Vector3.one * -.1f, .1f);
        }
    }
}
