using System;
using System.Collections;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float damageCooldown;
    [SerializeField] protected float deathCooldown = 10f;
    
    protected float _currentHealth;
    protected bool _canTakeDamage;
    protected Animator _anim;

    protected virtual void Start()
    {
        _anim.GetComponent<Animator>();
        _currentHealth = maxHealth;
        _canTakeDamage = true;
    }

    public void ApplyDamage(float damage)
    {
        if(!_canTakeDamage || _currentHealth <= 0f) return;
        
        _currentHealth -= damage;
        if (_currentHealth <= 0f)
        {
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
