using System;
using System.Collections.Generic;

public class StatsMediator
{
    private readonly LinkedList<StatModifier> modifiers = new();

    public event EventHandler<Query> Queries;
    public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);
    
    public void AddModifier(StatModifier modifier)
    {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };
    }

    public void Update(float deltaTime)
    {
        // Update all modifiers with deltaTime
        var node = modifiers.First;
        while (node != null)
        {
            // Get reference to modifier in the current node.
            var modifier = node.Value;
            modifier.Update(deltaTime);
            node = node.Next;
        }
        
        // Dispose any nodes that are marked for removal
        node = modifiers.First;
        while (node != null)
        {
            // get reference to the next node in case the current is marked for removeal.
            var nextNode = node.Next;

            if (node.Value.MarkedForRemoval)
            {
                node.Value.Dispose();
            }
            node = nextNode;
        }
    }
}

public class Query
{
    public readonly StatType StatType;
    public int Value;

    public Query(StatType statType, int value)
    {
        StatType = statType;
        Value = value;
    }
}