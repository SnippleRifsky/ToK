using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] public BaseStats baseStats;
    [SerializeField] protected int level;

    [SerializeField] protected string entityName = string.Empty;

    protected Entity _target;
    private CapsuleCollider _collider;
    public Stats Stats { get; protected set; }

    public int Level
    {
        get => level;
        protected set => level = value;
    }

    public string EntityName
    {
        get => entityName;
        protected set => entityName = value;
    }

    protected virtual void Awake()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
        _collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable()
    {
        if (_collider == null) return;
        CursorRaycastService.Instance.RegisterEntity(this, _collider as Collider);
    }

    protected virtual void OnDisable()
    {
        if (_collider != null)
        {
            CursorRaycastService.Instance.UnregisterEntity(_collider);
        }
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