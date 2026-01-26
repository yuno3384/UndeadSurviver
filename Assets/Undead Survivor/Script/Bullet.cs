using UnityEngine;

public class Bullet : MonoBehaviour
{
    //데미지
    public float damage;
    //관통변수
    public int per;

    public void Init(float damage , int per )
    {
        this.damage = damage;
        this.per = per;
    }



}
