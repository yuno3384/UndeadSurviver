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
    Animator anim;
    SpriteRenderer sr;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        // Animator가 없으면 추가
        if (anim == null)
        {
            anim = gameObject.AddComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive)
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
        sr.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
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
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null)
        {
            return;
        }

        health -= bullet.damage;

        if(health > 0)
        {
            // ... Live. Hit Action
        }
        else
        {
            // ... Die
            Dead();
        }

    }


    void Dead()
    {
        gameObject.SetActive(false);
    }

}
