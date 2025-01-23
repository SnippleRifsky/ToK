using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] public BaseStats baseStats;
    public Stats Stats { get; protected set; }
    [SerializeField] protected int level;
    public int Level
    {
        get => level;
        protected set => level = value;
    }

    [SerializeField] protected string entityName = string.Empty;
    public string EntityName
    {
        get => entityName;
        protected set => entityName = value;
    }
    protected Entity _target;

    protected virtual void Awake()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
    }

    protected virtual void Update()
    {
        Stats.Update(Time.deltaTime);
    }

    #region Health Methods

    public virtual void TakeDamage(float damage)
    {
        Stats.Resources.CurrentHealth -= damage;
    }

    public virtual void Heal(float heal)
    {
        Stats.Resources.CurrentHealth += heal;
    }

    #endregion
}