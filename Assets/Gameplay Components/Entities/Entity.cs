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

    private bool _isDying;

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
        Stats = new Stats(new StatsMediator(), baseStats, gameObject);
        _collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable()
    {
        if (_collider == null) return;
        CursorRaycastService.Instance.RegisterEntity(this, _collider);
    }

    protected virtual void OnDisable()
    {
        if (_collider != null) CursorRaycastService.Instance.UnregisterEntity(_collider);
    }

    protected virtual void Update()
    {
        Stats.Update(Time.deltaTime);
    }

    #region Health Methods

    public virtual void TakeDamage(float damage)
    {
        if (damage >= Stats.Resources.CurrentHealth)
        {
            Stats.Resources.CurrentHealth = 0;
            Die();
        }
        else
        {
            Stats.Resources.CurrentHealth -= damage;
        }
    }

    public virtual void Heal(float heal)
    {
        Stats.Resources.CurrentHealth += heal;
    }

    #endregion

    public virtual void Die()
    {
        if (_isDying) return;
        _isDying = true;

        try
        {
            EventBus.Publish(new EntityEvents.EntityDeathEvent(this));

            var components = GetComponents<IDestructible>();
            foreach (var component in components) component.OnDestroy();

            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during entity death: {e.Message}");
            _isDying = false;
        }
    }
}