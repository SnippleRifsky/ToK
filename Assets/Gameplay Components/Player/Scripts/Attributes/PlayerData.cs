using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
    {
        [SerializeField] public BaseStats baseStats;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnResourceChanged;
        public Stats Stats { get; private set; }

        void Awake()
        {
            Stats = new Stats(new StatsMediator(), baseStats);
        }

        public void Update()
        {
            Stats.Update(Time.deltaTime);
        }
        
        public void TakeDamage(float damage)
        {
            Stats.Resources.CurrentHealth -= damage;
            OnHealthChanged?.Invoke(Stats.Resources.CurrentHealth);
        }

        public void Heal(float heal)
        {
            Stats.Resources.CurrentHealth += heal;
            OnHealthChanged?.Invoke(Stats.Resources.CurrentHealth);
        }

        public void SpendResource(float amount)
        {
            Stats.Resources.CurrentResource -= amount;
            OnResourceChanged?.Invoke(Stats.Resources.CurrentResource);
        }
    }