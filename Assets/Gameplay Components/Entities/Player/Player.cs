using UnityEngine;

public class Player : Entity
    {
        public PlayerController PlayerController { get; private set; }
        public CameraController CameraController { get; private set; }

        private void SetupPlayer()
        {
            PlayerController = gameObject.AddComponent<PlayerController>();
            
            // Iterate children to find by name or component if needed
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child.name != "CameraRig") continue;
                CameraController = child.AddComponent<CameraController>();
                break;
            }
        }

        protected override void Awake()
        {
            SetupPlayer();
            Stats = new Stats(new StatsMediator(), baseStats);
        }
        
        // Probably move to ability system once implemented
        public void SpendResource(float amount)
        {
            Stats.Resources.CurrentResource -= amount;
        }
    }