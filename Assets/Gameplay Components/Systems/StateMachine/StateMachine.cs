using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine
{
    private StateNode current;
    private Dictionary<Type, StateNode> nodes = new();
    private HashSet<ITransition> anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }
        current.State?.Update();
    }
    
    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.State?.OnEnter();
    }
    
    void ChangeState(IState state)
    {
        if (state == current.State) return;
        
        var previous = current.State;
        var next = nodes[state.GetType()].State;
        
        previous?.OnExit();
        next?.OnEnter();
        current = nodes[state.GetType()];
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }
    
    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node != null) return node;
        node = new StateNode(state);
        nodes[state.GetType()] = node;

        return node;
    }
    
    ITransition GetTransition()
    {
        foreach (var transition in anyTransitions.Where(transition => transition.Condition.Evaluate()))
        {
            return transition;
        }

        return current.Transitions.FirstOrDefault(transition => transition.Condition.Evaluate());
    }

    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}