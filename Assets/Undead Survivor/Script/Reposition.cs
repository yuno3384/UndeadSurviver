using UnityEngine;



public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private const int MAPSIZE = 40;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);
        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY) // X축 우선 이동
                {
                    transform.Translate(Vector3.right * dirX * MAPSIZE);
                }
                else if (diffX < diffY) // Y축 우선 이동
                {
                    transform.Translate(Vector3.up * dirY * MAPSIZE);
                }
                else // 대각선 이동 (diffX == diffY)
                {
                    transform.Translate(Vector3.right * dirX * MAPSIZE + Vector3.up * dirY * MAPSIZE);
                }
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}


//## 주요 변경사항

//1. ✅ `Awake()`에서 `coll = GetComponent<Collider2D>()` 추가
//2. ✅ `diffX == diffY` 경우(대각선) 처리 추가
//3. ✅ 하드코딩된 40을 `MAPSIZE` 상수로 변경

//이제 플레이어가 대각선으로 이동할 때도 맵 타일이 제대로 재배치될 것입니다!