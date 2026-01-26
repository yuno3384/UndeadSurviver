using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float _speed = 3f;
    public Scanner scanner;

    Rigidbody2D rd;
    SpriteRenderer sr;
    Animator anim;

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    private void Start()
    {
        
    }

    void FixedUpdate()
    {
        //// 1. ���� �ش� > AddForce
        //rd.AddForce(inputVec);
        // // 2. �ӵ� ����
        // rd.linearVelocity = inputVec;

        Vector2 nextVec = inputVec.normalized * _speed * Time.fixedDeltaTime;

        // 3. ��ġ �̵�
        rd.MovePosition(rd.position + nextVec);
    }


    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();

    }

    void LateUpdate()
    {
        anim.SetFloat("Speed",inputVec.magnitude);
        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }  
    }

}
