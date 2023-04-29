using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.Controller;
using RoguetyCraft.Enemy.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Enemy.States
{
    public abstract class EnemyState : State
    {
        protected EnemyController _enemy;
        public EnemyState(EnemyController enemy)
        {
            _enemy = enemy;
        }
    }
    public class EnemyIdle : EnemyState
    {
        public EnemyIdle(EnemyController enemy) : base(enemy) { }
        public override void Handle() {}
        public override void OnEnter() {}
        public override void OnExit() {}
    }
    public class EnemyPatrol : EnemyState
    {
        public EnemyPatrol(EnemyController enemy) : base(enemy) { }
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyChase : EnemyState
    {
        public EnemyChase(EnemyController enemy) : base(enemy) { }
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyAttack : EnemyState
    {
        public EnemyAttack(EnemyController enemy) : base(enemy) { }
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public class EnemyDead : EnemyState
    {
        public EnemyDead(EnemyController enemy) : base(enemy) { }
        public override void Handle() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
}
