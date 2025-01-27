using System;
using UnityEngine;

public class PlayerData : MonoBehaviour, IResourceProvider
    {
        [SerializeField] public BaseStats baseStats;
        private Stats _stats;

        void Awake()
        {
            _stats = new Stats(new StatsMediator(), baseStats);
        }
        
        public float CurrentHealth => _stats.Resources.CurrentHealth;
        public float MaxHealth => _stats.MaxHealth;
        public event Action<float> OnHealthChanged
        {
            add => _stats.Resources.OnHealthChanged += value;
            remove => _stats.Resources.OnHealthChanged -= value;
        }
        
        public float CurrentResource => _stats.Resources.CurrentResource;
        public float MaxResource => _stats.MaxResource;
        public event Action<float> OnResourceChanged
        {
            add => _stats.Resources.OnResourceChanged += value;
            remove => _stats.Resources.OnResourceChanged -= value;
        }

        public void Update()
        {
            _stats.Update(Time.deltaTime);
        }
        
        public void TakeDamage(float damage)
        {
            _stats.Resources.CurrentHealth -= damage;
        }

        public void Heal(float healAmount)
        {
            _stats.Resources.CurrentHealth += healAmount;
        }

        public void SpendResource(float resourceCost)
        {
            _stats.Resources.CurrentResource -= resourceCost;
        }
    }