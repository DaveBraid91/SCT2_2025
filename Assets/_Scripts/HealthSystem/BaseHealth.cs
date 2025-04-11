using System;
using System.Collections;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float damageCooldown;
    [SerializeField] protected float deathCooldown = 10f;
    [SerializeField] protected LifeBarUI lifeBarUI;
    
    public float CurrentHealth { get; protected set; }

    public float MaxHealth => maxHealth;

    protected bool _canTakeDamage;
    protected Animator _anim;

    protected virtual void Start()
    {
        _anim = GetComponent<Animator>();
        CurrentHealth = maxHealth;
        _canTakeDamage = true;
    }

    public void ApplyDamage(float damage)
    {
        if(!_canTakeDamage || CurrentHealth <= 0f || damage <= 0) return;
        
        Debug.Log($"Health pre-damage: {CurrentHealth}");
        CurrentHealth -= damage;
        lifeBarUI.UpdateLifeBar(this);
        Debug.Log($"Health post-damage: {CurrentHealth}");
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            Die();
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        _canTakeDamage = true;
    }

    protected abstract void Die();

}
