using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * 2023/07/18
 * 3D RPG Project
 * 강아름
 * 
 * Monster Script
 * 몬스터 AI의 FSM. AI 상태 변경 및 각각의 상태에 진입 시 일어나는 실제 행동 구현.
 */

public class MonsterManager : MonoBehaviour
{
    #region Singleton

    public static MonsterManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField]
    private new string name;
    [SerializeField]
    private int hp;
    [SerializeField]
    private int power;

    public string Name { get => name; set => name = value; }
    public int Hp { get => hp; set => hp = value; }
    public int Power { get => power; set => power = value; }

    //상태 구현을 위한 추상 클래스
    public abstract class BaseState
    {
        protected MonsterManager _monster;

        protected BaseState(MonsterManager monster)
        {
            _monster = monster;
        }

        public abstract void OnStateEnter();    //처음 상태에 진입 시 호출
        public abstract void OnStateUpdate();   //매프레임마다 호출
        public abstract void OnStateExit();     //상태 변경 시 호출
    }

    //현재 상태 혹은 상태 변경 시 호출되는 메서드 관리, 상태 제어
    public class FSM
    {
        private BaseState _curState;

        public FSM(BaseState initState)
        {
            _curState = initState;
        }
        public void ChangeState(BaseState nextState)
        {
            if (nextState == _curState) return;
            if (_curState != null)
                _curState.OnStateExit();
            _curState = nextState;
            _curState.OnStateEnter();
        }

        public void UpdateState()
        {
            if (_curState != null)
                _curState.OnStateUpdate();
        }
    }

    //실제 행동 구현
    public class IdleState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public IdleState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", false);

            agent = _monster.GetComponent<NavMeshAgent>();
            agent.velocity = Vector3.zero;
            agent.enabled = true;
        }
        public override void OnStateUpdate()
        {
               
        }
        public override void OnStateExit()
        {
          
        }
    }

    public class WalkState : BaseState
    {
        Animator animator;
        public WalkState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", true);
        }
        public override void OnStateUpdate()
        {
          
        }
        public override void OnStateExit()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", false);
        }
    }
    public class TraceState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        Transform target = PlayerManager.instance.player.transform;
        public TraceState(MonsterManager monster) : base(monster) { }

        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", true);
        }
        public override void OnStateUpdate()
        {
            agent = _monster.GetComponent<NavMeshAgent>();
            agent.SetDestination(target.position);
        }
        public override void OnStateExit()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", false);
        }
    }
    public class AttackState : BaseState
    {
        Animator animator;
        public AttackState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Attack", true);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Attack", false);
        }
    }
    public class DieState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public DieState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Die", true);

            agent = _monster.GetComponent<NavMeshAgent>();
            agent.enabled = false;
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Die", false);
        }
    }
    //보스
    public class SnowRockState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public SnowRockState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("SnowRockAttack", true);

            agent = _monster.GetComponent<NavMeshAgent>();
            agent.isStopped = true;

            ObjectPoolManager.instance.Snowpool.Allpop();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("SnowRockAttack", false);

            agent.isStopped = false;
        }
    }
    public class ForWardState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public ForWardState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("ForwardAttack", true);
            agent = _monster.GetComponent<NavMeshAgent>();
            agent.speed = 8f;
        }
        public override void OnStateUpdate()
        {
            agent.enabled = false;
            Vector3 dir = _monster.transform.forward;
            _monster.transform.position += dir * agent.speed * Time.deltaTime;
        }
        public override void OnStateExit()
        {
            animator.SetBool("ForwardAttack", false);
            agent.speed = 1.0f;
            agent.autoRepath = true;
            agent.enabled = true;
            agent.velocity = Vector3.zero;
        }
    }
    public class SphereAttackState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public SphereAttackState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("SphereAttack", true);

            agent = _monster.GetComponent<NavMeshAgent>();
            agent.isStopped = true;
        }
        public override void OnStateUpdate()
        {
        }
        public override void OnStateExit()
        {
            animator.SetBool("SphereAttack", false);
            agent.isStopped = false;
        }
    }
    public class WaitState : BaseState
    {
        Animator animator;
        NavMeshAgent agent;
        public WaitState(MonsterManager monster) : base(monster) { }
        public override void OnStateEnter()
        {
            animator = _monster.GetComponent<Animator>();
            animator.SetBool("Walk", false);

            agent = _monster.GetComponent<NavMeshAgent>();
            agent.isStopped = true;
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            agent.isStopped = false;
        }
    }
}
