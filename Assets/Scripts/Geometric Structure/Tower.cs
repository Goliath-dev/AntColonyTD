using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower 
{
    public TwoDimPoint Position { get; private set; }
    public int Distance { get; private set; }
    public int Damage { get; private set; }

    private const int targetCapacity = 1;
    private HashSet<TwoDimAnt> targets = new HashSet<TwoDimAnt>(); //Targets that are being shot now.
    private HashSet<TwoDimAnt> possibleTargets = new HashSet<TwoDimAnt>(); //Targets within area of effect. 

    public Tower(TwoDimPoint position, int distance, int damage)
    {
        Position = position;
        Distance = distance;
        Damage = damage;
    }

    public void AddPossibleTarget(TwoDimAnt target)
    {
        possibleTargets.Add(target);
        if (targets.Count < targetCapacity) targets.Add(target);
    }

    public void Shoot()
    {
        foreach (TwoDimAnt target in targets)
        {
            target.Ant.Health -= Damage;
        }
    }
}
