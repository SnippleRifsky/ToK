using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
    {
        [SerializeField] public BaseStats baseStats;
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
        }

        public void Heal(float heal)
        {
            Stats.Resources.CurrentHealth += heal;
        }

        public void SpendResource(float amount)
        {
            Stats.Resources.CurrentResource -= amount;
        }
    }