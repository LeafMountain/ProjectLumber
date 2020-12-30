using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class Health : MonoBehaviour, IInteractable
{
    public UnityEvent OnDead;
    public int health;
    public int maxHealth = 10;
    public float percentage => health / (float)maxHealth;

    public Healthbar healthbar;

    new private Collider collider;
    private IEnumerator healthbarTimer;

    private void Awake()
    {
        health = maxHealth;
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Addressables.InstantiateAsync("UI/Healthbar", UI.Instance.transform).Completed += (result) =>
        {
            healthbar = result.Result.GetComponent<Healthbar>();
            healthbar.slider.value = percentage;
            healthbarTimer = HealthbarTimer();
            healthbar.gameObject.SetActive(false);
        };
    }

    private void LateUpdate()
    {
        if (healthbar)
        {
            healthbar.transform.position = Camera.main.WorldToScreenPoint(collider.bounds.max);
        }
    }

    private void OnDestroy()
    {
        if (healthbar)
        {
            Destroy(healthbar.gameObject);
        }
    }

    public void Damage(int value, Interactable instigator)
    {
        health += value;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (value < 0)
        {
            transform.DOPunchScale(Vector3.one * -.1f, .1f);
        }
        healthbar.slider.value = percentage;
        healthbar.gameObject.SetActive(true);
        StopCoroutine(healthbarTimer);
        StartCoroutine(healthbarTimer);
        UI.ShowNotification(GetComponent<Interactable>(), value.ToString());
        if (health == 0)
        {
            OnDead?.Invoke();
            Destroy(gameObject);
        }
    }

    public IEnumerator AttackInteraction(Unit unit)
    {
        yield return unit.navAgent.MoveToInteractableSeq(GetComponent<Interactable>());
        yield return unit.LookAt(GetComponent<Interactable>());
        unit.animator.SetTrigger("attackMelee");
        yield return new WaitForSeconds(.5f);
        Damage(-1, unit.interactable);
        yield return new WaitForSeconds(.2f);
    }

    public bool IsEnabled()
    {
        if(InteractionSystem.Instance.interactables.Count == 0 || InteractionSystem.Instance.interactables[0] == GetComponent<Interactable>())
        {
            return false;
        }
        return true;
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        yield return AttackInteraction(unit);
    }

    public string GetName()
    {
        return "Attack";
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Icons/Placeholders/Sword");
    }

    public IEnumerator HealthbarTimer()
    {
        yield return new WaitForSeconds(3f);
        healthbar.gameObject.SetActive(false);
    }
}
