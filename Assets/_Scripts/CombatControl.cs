using System;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int StrongAttack = Animator.StringToHash("strongAttack");
    [SerializeField] private Collider weaponTrigger;
    
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        
        weaponTrigger.enabled = false;
    }

    private void Update()
    {
        if (Input.GetAxis("Fire1") > 0.5f)
        {
            _anim.SetTrigger(Attack);
        }
        else
        {
            _anim.ResetTrigger(Attack);
        }
        if (Input.GetAxis("Fire2") > 0.5f)
        {
            _anim.SetTrigger(StrongAttack);
        }
        else
        {
            _anim.ResetTrigger(StrongAttack);
        }
        
    }

    private void ActivateTriggerAttack()
    {
        weaponTrigger.enabled = true;
    }
    
    private void DisableTriggerAttack()
    {
        weaponTrigger.enabled = false;
    }
}
