using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
{
    [SerializeField] private Image bar;
    
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main?.transform;
    }

    private void Update()
    {
        transform.LookAt(_cameraTransform);
    }

    public void UpdateLifeBar(BaseHealth characterHealth)
    {
        var currentHealth = characterHealth.CurrentHealth;
        var maxHealth = characterHealth.MaxHealth;
        var lifePercentage = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        bar.fillAmount = lifePercentage;
    }
}
