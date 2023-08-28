using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2023/07/17
 * 3D RPG Project
 * 강아름
 * 
 * Player Script
 * 캐릭터의 모든 것을 관리
 */

public class PlayerController : MonoBehaviour
{ 
    private CharacterController col;    //캐릭터 컨트롤러
    private Animator anim;              //애니메이션

    public GameObject SwordPos;

    public float speed;                 // 캐릭터 움직임 스피드
    public float rotateSpeed;           // 캐릭터 회전 스피드
    public float jumpSpeed;             // 캐릭터 점프 힘
    private float gravity = -9.81f;     // 캐릭터에게 작용하는 중력 
    private float attackSpeed;

    private float hitPower = 1.5f;
    private bool ishit = false;

    private Vector3 MoveDir;            // 캐릭터의 움직이는 방향
    private float h, v;

    [SerializeField]
    private AudioSource[] audioSource;  //0 : 발소리, 1 : 검소리, 2 : 점프

    void Start()
    {
        col = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        MoveDir = Vector3.zero;
    }

    void Update()
    {
        //die
        if (PlayerManager.instance.Player_HP <= 0)
        {
            PlayerManager.instance.Player_HP = 0;
            PlayerManager.instance._hpbar.SetMaxHp(0);
            anim.SetBool("Die", true);
            StartCoroutine(Die());
        }

        //WSAD 움직임은 돌아가기 (방향키만 움직임으로 인정), 대화 중일 때는 움직일 수 없음
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
            DialogueTrigger.isTalking || CameraController.MyInstance.MoveBoolCheck() || PlayerManager.instance.Player_HP <= 0)
        {
            audioSource[0].Stop();
            return;
        }

        if (CameraController.MyInstance.EnterBossRoom())
            PlayerManager.instance._hpbar.SetTrue();

        if (col.isGrounded)
        {
            //move
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            MoveDir = new Vector3(h, 0, v).normalized * speed;
            //카메라가 바라보는 방향으로 전환
            MoveDir = CameraController.MyInstance.transform.TransformDirection(MoveDir);

            Rotation();

            anim.SetBool("Jump", false);
            anim.SetBool("Attack", false);

            attackSpeed += Time.deltaTime;

            if (ishit)
            {
                MoveDir.y = hitPower;
                MoveDir.z = hitPower;
            }
            if (Input.GetButtonDown("Jump"))
            {
                audioSource[2].PlayOneShot(audioSource[2].clip);
                MoveDir.y = jumpSpeed;
                anim.SetBool("Jump", true);
            }
            //attack
            if (Input.GetKeyDown(KeyCode.C) && attackSpeed > PlayerManager.instance.AttackDelay)
            {
                audioSource[1].PlayOneShot(audioSource[1].clip);

                //검 생성 및 생성 위치, 방향
                ObjectPoolManager.instance.Swordpool.Pop(SwordPos.transform.position,
                    Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f));
                anim.SetBool("Attack", true);
                attackSpeed = 0;
            }
           
        }
         //점프시 중력 적용
        MoveDir.y += gravity * Time.deltaTime;
        //캐릭터 실제 움직임
        col.Move(MoveDir * Time.deltaTime);
        //움직임 발생시 움직이는 애니 실행
        anim.SetFloat("Speed", MoveDir.magnitude);
        ishit = false;

        if (!audioSource[0].isPlaying && MoveDir.magnitude >= 0.3f && !audioSource[2].isPlaying)
            audioSource[0].PlayOneShot(audioSource[0].clip);
        if (MoveDir.magnitude < 0.3f)
            audioSource[0].Stop();
    }

    void Rotation()
    {
        //오른쪽
        if (Input.GetAxisRaw("Horizontal") >= 1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir), rotateSpeed * Time.deltaTime);
        }

        //왼쪽
        if (Input.GetAxisRaw("Horizontal") <= -1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir), rotateSpeed * 50f * Time.deltaTime);
        }

        //위
        if (Input.GetAxisRaw("Vertical") >= 1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir), rotateSpeed * Time.deltaTime);
        }

        //아래
        if (Input.GetAxisRaw("Vertical") <= -1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir), rotateSpeed * 50f * Time.deltaTime) ;
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(4.0f);
        PlayerManager.instance.Player_HP = 100;
        PlayerManager.instance._hpbar.SetMaxHp(100);
        anim.SetBool("Die", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "boss" || other.gameObject.tag == "SnowRock" || other.gameObject.tag == "Sphere")
        {
            ishit = true;
        }
    }
}
