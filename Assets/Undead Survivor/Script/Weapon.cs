using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // �� ������ ���� ��ȣ
    public int prefabId; // �� �ش��ϴ� �������� ������ȣ
    public float damage; // �� ���Ⱑ �ִ� ������
    public int count; // �� ���⸦ ������ �θ� ���
    public float speed; // ȸ���ӵ�

    float timer;
    Player player;


    private void Awake()
    {
       player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //.. Test...
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }
    }


    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f;
                break;
        }
    }


    void Batch()
    {
        // Null 체크 추가
        if (GameManager.instance == null || GameManager.instance.pool == null)
        {
            return;
        }

        for(int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            { 
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }


            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.Init(damage, -1); // -1 is Infinity Per.
            }
        }
    }

    void Fire()
    {
       if(!player.scanner.nearestTarget)
       {
            return;
       }

        Debug.Log("Fire 호출됨!"); // 이 로그가 찍히는지 확인

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        Debug.Log("총알 스폰됨: " + bullet.name); // 총알이 생성되는지 확인


    }


}
