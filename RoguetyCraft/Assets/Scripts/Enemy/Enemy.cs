using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.States;

namespace RoguetyCraft.Enemy.Generic
{
    public enum EnemyStates
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        HIT,
        DEAD
    }
}
