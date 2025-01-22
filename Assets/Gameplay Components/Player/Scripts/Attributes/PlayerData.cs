using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
            Stats.Mediator.Update(Time.deltaTime);
        }

        public void Start()
        {
            Debug.Log(Stats.ToString());
        }
    }