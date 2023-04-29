using RoguetyCraft.DesignPatterns.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Enemy.States
{
    public class EnemyIdle : State
    {
        public override void Handle() {}
        public override void OnEnter() {}
        public override void OnExit() {}
    }
    public class EnemyPatrol : State
    {
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyChase : State
    {
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyAttack : State
    {
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyDead : State
    {
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
}
