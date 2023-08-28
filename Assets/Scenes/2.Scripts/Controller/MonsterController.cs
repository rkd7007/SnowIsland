using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * 2023/07/18
 * 3D RPG Project
 * 강아름
 * 
 * MonsterController Script
 * 몬스터 AI의 상태 변경 관리
 */

public class MonsterController : MonsterManager
{
    private enum State
    {
        Idle,
        Walk,
        Trace,
        Attack,
        Die
    }

    private State _curState;
    private FSM _fsm;

    NavMeshAgent agent;
    Transform target;

    public float lookRadius;
    public float nearRadius;

    private float attackTimer;
    private float attackDelay = 1.1f; //0.75

    private Vector3 oriPos;

    [SerializeField] HpBar _hpbar;

    void Start()
    {
        _curState = State.Idle;
        _fsm = new FSM(new IdleState(this));

        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform; //캐릭터 정보 받아옴

        oriPos = transform.position;
        _hpbar.SetFalse();
    }

    void Update()
    {
        switch (_curState)
        {
            case State.Idle:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (SeeTarget() && PlayerManager.instance.Player_HP > 0)
                    {
                        ChangeState(State.Trace);
                    }
                    else
                        ChangeState(State.Idle);
                }
                break;
            case State.Walk:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (Vector3.Distance(oriPos, transform.position) >= 1.0f)
                    {
                        //원래 위치로 돌아가기
                        _hpbar.SetFalse();
                        agent.SetDestination(oriPos);
                    }
                    else
                        ChangeState(State.Idle);
                }
                break;
            case State.Trace:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if(SeeTarget() && PlayerManager.instance.Player_HP > 0) //캐릭터 목격
                    {
                        StopCoroutine(TraceToWalkDeley());
                        _fsm.UpdateState();

                        if (NearTarget()) //캐릭터가 공격 가능 범위 내에 있음
                        {
                            ChangeState(State.Attack);
                        }
                    }
                    else
                    {
                        //일정 시간을 준 뒤 제자리로 돌아가도록 함
                        StartCoroutine(TraceToWalkDeley());
                    }
                }
                break;
            case State.Attack:
                {
                    if (Hp <= 0)
                    {
                        GameManager.MyInstance.KillConfirmed(this);
                        ChangeState(State.Die);
                    }
                    else if (NearTarget())
                    {
                        if (CameraController.MyInstance.MoveBoolCheck())
                            return;

                        //밀리는 현상 제거
                        agent.velocity = Vector3.zero;
                        //일정 시간이 지나야만(= 공격 애니메이션이 끝나는 시간으로 맞춤) 공격으로 인정
                        attackTimer += Time.deltaTime;

                        if (PlayerManager.instance.Player_HP > 0 && attackTimer > attackDelay)
                        {
                            //데미지 Text 출력
                            TextManager.MyInstance.PopupPlayerTakeDmgText(Power);
                            PlayerManager.instance.PlayerTakeDmg(Power);
                            attackTimer = 0;
                        }
                        else if (PlayerManager.instance.Player_HP <= 0)
                        {
                            ChangeState(State.Walk);
                        }
                    }
                    else
                        ChangeState(State.Trace);
                }
                break;
            case State.Die:
                {
                    if (Hp <= 0)
                    {
                        _hpbar.SetFalse();
                        ChangeState(State.Die);

                        StartCoroutine(WaitDieDeley());
                    }
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            //데미지 Text 출력
            TextManager.MyInstance.PopupMonsterTakeDmgText(PlayerManager.instance.Player_Power, transform.position);
            Hp -= PlayerManager.instance.Player_Power;
            _hpbar.SetTrue();
            _hpbar.SetHp(Hp);
        }
    }

    private void ChangeState(State nextState)
    {
        _curState = nextState;
        switch (_curState)
        {
            case State.Idle:
                _fsm.ChangeState(new IdleState(this));
                break;
            case State.Walk:
                _fsm.ChangeState(new WalkState(this));
                break;
            case State.Trace:
                _fsm.ChangeState(new TraceState(this));
                break; 
            case State.Attack:
                 _fsm.ChangeState(new AttackState(this));
                break;
            case State.Die:
                 _fsm.ChangeState(new DieState(this));
                break;
        }
    }

    private bool SeeTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            return true;
        }
        else return false;
    }

    private bool NearTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= nearRadius)
        {
            return true;
        }
        else return false;
    }

    IEnumerator TraceToWalkDeley()
    {
        yield return new WaitForSeconds(1.3f);
        ChangeState(State.Walk);
    }
    IEnumerator WaitDieDeley()
    {
        //죽는 모션 끝날 때 까지 대기
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
        ChangeState(State.Idle);
        //AI 초기화 후 재생성. 코루틴은 비활성화 상태에서는 실행되지 않기에 Invoke 활용
        Invoke("DieToIdelDeley", 5.5f); 
    }

    private void DieToIdelDeley()
    {
        transform.position = oriPos;
        Hp = 100;
        _hpbar.SetMaxHp(Hp);
        gameObject.SetActive(true);
        agent.velocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, nearRadius);
    }
}
