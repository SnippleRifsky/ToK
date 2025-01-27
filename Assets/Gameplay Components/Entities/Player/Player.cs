using System;

public class Player : Entity, IResourceProvider
{
    public PlayerController PlayerController { get; private set; }
    public CameraController CameraController { get; private set; }
    public CharacterLeveling CharacterLeveling { get; private set; }

    protected override void Awake()
    {
        SetupPlayer();
        Stats = new Stats(new StatsMediator(), baseStats);
    }
    
    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;
    public event Action<float> OnHealthChanged
    {
        add => Stats.Resources.OnHealthChanged += value;
        remove => Stats.Resources.OnHealthChanged -= value;
    }
        
    public float CurrentResource => Stats.Resources.CurrentResource;
    public float MaxResource => Stats.MaxResource;
    public event Action<float> OnResourceChanged
    {
        add => Stats.Resources.OnResourceChanged += value;
        remove => Stats.Resources.OnResourceChanged -= value;
    }

    private void SetupPlayer()
    {
        PlayerController = gameObject.AddComponent<PlayerController>();
        CharacterLeveling = gameObject.AddComponent<CharacterLeveling>();

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child.name != "CameraRig") continue;
            CameraController = child.AddComponent<CameraController>();
            break;
        }
    }

    // Probably move to ability system once implemented
    public void SpendResource(float amount)
    {
        Stats.Resources.CurrentResource -= amount;
    }

    public void AddXp(int amount)
    {
        CharacterLeveling.AddXp(amount);
    }
}