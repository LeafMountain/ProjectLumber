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
        UINotification.ShowNotification(GetComponent<Interactable>(), value.ToString());
    }

    public IEnumerator AttackInteraction(Unit unit)
    {
        yield return unit.navAgent.MoveToInteractable(GetComponent<Interactable>());
        yield return unit.LookAt(GetComponent<Interactable>());
        unit.animator.SetTrigger("attackMelee");
        yield return new WaitForSeconds(.5f);
        Damage(-1, unit.interactable);
        yield return new WaitForSeconds(.2f);
    }
}
