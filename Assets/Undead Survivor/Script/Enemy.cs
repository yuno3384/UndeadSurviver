using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;


    bool isAlive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        
        // Animator가 없으면 추가
        if (anim == null)
        {
            anim = gameObject.AddComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        // GetCurrentAnimatorStateInfo : 애니메이터의 현 상태 정보를 받는 것 > base는 0번
        //IsName : 이름이 일치하는가
        {
            return;
        }
        
        
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isAlive)
        {
            return;
        }
        
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        coll.enabled = true;
        rigid.simulated = true; // rigid.simulated : 리지드바디의 물리 비활성화
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        isAlive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        // Animator가 있고 animCon 배열이 유효한 경우에만 설정
        if (anim != null && animCon != null && data.spriteType >= 0 && data.spriteType < animCon.Length && animCon[data.spriteType] != null)
        {
            anim.runtimeAnimatorController = animCon[data.spriteType];
        }
        speed = data.speed;
        maxHealth = data.health;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null)
        {
            return;
        }

        health -= bullet.damage;
        //Knockback(); // 코루틴은 StartCoroutine으로 실행가능
        //StartCoroutine("Knockback");
        StartCoroutine(Knockback());

        if (health > 0)
        {
            // ... Live. Hit Action
            anim.SetTrigger("Hit");
        }
        else
        {
            // ... Die
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false; // rigid.simulated : 리지드바디의 물리 비활성화
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);
            //Dead();
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }

    }

    // 코루틴 : 생명주기와 비동기처럼 실행되는 함수 > IEnumerator
    IEnumerator Knockback()
    {
        //yield return null; // 1프레임 쉬기 > 하나의 물리 프레임을 딜레이
        //yield return new WaitForSeconds(2f); // 2초 쉬고 실행 > new이므로 최적화에 안 좋음 
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; // 플레이어 기준의 반대 방향
                                                         
        // 요약:
        //     Apply a force to the rigidbody.
        //
        // 매개 변수:
        //   force:
        //     Components of the force in the X and Y axes.
        //
        //   mode:
        //     The method used to apply the specified force.
        rigid.AddForce(dirVec.normalized* 3, ForceMode2D.Impulse);


    }


    void Dead()
    {
        gameObject.SetActive(false);
    }

}
