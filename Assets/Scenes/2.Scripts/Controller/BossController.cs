using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonsterManager
{
    private enum State
    {
        Idle,
        Trace,
        SnowRock,
        forWard,
        SphereAttack,
        Die,
        Wait
    }

    private State _curState;
    private FSM _fsm;

    private Transform target;

    private bool forwardWait = false;
    private bool SphereWait = false;
    private bool isAttack = false;

    public float lookRadius;

    [SerializeField] private GameObject projector;
    [SerializeField] private GameObject SpherePoint;

    [SerializeField] private HpBar _hpbar;

    void Start()
    {
        _curState = State.Idle;
        _fsm = new FSM(new IdleState(this));

        target = PlayerManager.instance.player.transform; //캐릭터 정보 받아옴

        projector.SetActive(false);
        SpherePoint.SetActive(false);

        _hpbar.SetFalse();

        SpherePoint = Instantiate(SpherePoint, this.gameObject.transform);
    }

    void Update()
    {
        if (CameraController.MyInstance.EnterBossRoom())
            _hpbar.SetTrue();

        switch (_curState)
        {
            case State.Idle:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (PlayerManager.instance.Player_HP > 0 && !isAttack && !forwardWait && !SphereWait
                        && CameraController.MyInstance.EnterBossRoom())
                    {
                        ChangeState(State.Trace);
                        StartCoroutine(ThinkDeley());
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
                    else if (PlayerManager.instance.Player_HP > 0 && !isAttack && !forwardWait && !SphereWait
                        && CameraController.MyInstance.EnterBossRoom())
                    {
                        _fsm.UpdateState();
                    }
                    else
                        ChangeState(State.Idle);
                }
                break;
            case State.SnowRock:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (PlayerManager.instance.Player_HP > 0)
                    {
                        StartCoroutine(SnowRockToIdleDeley());
                    }
                    else
                        ChangeState(State.Idle);
                }
                break;
            case State.forWard:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (forwardWait && PlayerManager.instance.Player_HP > 0)
                    {
                        _fsm.UpdateState();
                    }
                    else
                    {
                        ChangeState(State.Idle);
                        isAttack = false;
                    }
                }
                break;
            case State.SphereAttack:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (!SpherePoint.activeSelf || !SphereWait || PlayerManager.instance.Player_HP <= 0)
                    {
                        SphereWait = false;
                        ChangeState(State.Idle);
                        isAttack = false;
                    }
                }
                break;
            case State.Die:
                {
                    if (Hp <= 0)
                    {
                        //초기화
                        projector.SetActive(false);
                        SpherePoint.SetActive(false);

                        BGMSound.instance.ChangeBGM(2);
                        _hpbar.SetFalse();
                        CameraController.MyInstance.BossDieChangeCamera();

                        //점차 작아지기
                        float ScaleValue = 1.0f;
                        Vector3 pos = transform.localScale;
                        ScaleValue -= 0.01f;
                        transform.localScale = pos * ScaleValue;

                        StartCoroutine(DieDeley());
                    }
                }
                break;
            case State.Wait:
                {
                    if (Hp <= 0)
                    {
                        ChangeState(State.Die);
                    }
                    else if (PlayerManager.instance.Player_HP > 0)
                    {
                        StartCoroutine(WaitToforWardDeley());
                    }
                    else
                    {
                        ChangeState(State.Idle);
                        isAttack = false;
                    }
                }
                break;
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
            case State.Trace:
                _fsm.ChangeState(new TraceState(this));
                break;
            case State.SnowRock:
                _fsm.ChangeState(new SnowRockState(this));
                break;
            case State.forWard:
                _fsm.ChangeState(new ForWardState(this));
                break;
            case State.SphereAttack:
                _fsm.ChangeState(new SphereAttackState(this));
                break;
            case State.Die:
                _fsm.ChangeState(new DieState(this));
                break;
            case State.Wait:
                _fsm.ChangeState(new WaitState(this));
                break;
        }
    }

    IEnumerator ThinkDeley()
    {
        yield return new WaitForSeconds(3.0f);

        int ranAction = Random.Range(0, 5);

        switch (ranAction)
        {
            case 0:
                {
                    if (!isAttack)
                    {
                        //눈덩이 던지기
                        isAttack = true;
                        ChangeState(State.SnowRock);
                    }    
                }
                break;
            case 1:
            case 2:
                {
                    if (!isAttack)
                    {
                        //돌진
                        isAttack = true;
                        forwardWait = true;
                        projector.SetActive(true);
                        ChangeState(State.Wait);
                    }
                }
                break;
            case 3:
            case 4:
                {
                    if (!isAttack)
                    {
                        //광역 공격
                        isAttack = true;
                        SphereWait = true;
                        SpherePoint.SetActive(true);
                        ChangeState(State.SphereAttack);
                    }
                }
                break;
        }
    }
    IEnumerator SnowRockToIdleDeley()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(State.Trace);
        StartCoroutine(ThinkDeley());
        isAttack = false;
    }

    IEnumerator WaitToforWardDeley()
    {
        yield return new WaitForSeconds(1.0f);
        ChangeState(State.forWard);
    }

    IEnumerator DieDeley()
    {
        yield return new WaitForSeconds(4.0f);
        gameObject.SetActive(false);
        Potal.instance.PotalCreate();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            forwardWait = false;
            projector.SetActive(false);
        }

        if (other.gameObject.tag == "Player")
        {
            if (forwardWait)
            {
                forwardWait = false;
                projector.SetActive(false);
            }
            if (SphereWait)
            {
                SphereAttack.MyInstance.Reset();
                SphereWait = false;
                SpherePoint.SetActive(false);
            }

            TextManager.MyInstance.PopupPlayerTakeDmgText(Power);
            PlayerManager.instance.PlayerTakeDmg(Power);
            isAttack = false;
        }
    }
}
