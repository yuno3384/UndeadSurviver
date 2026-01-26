using UnityEngine;
using UnityEngine.Rendering;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets; // 여러 적을 검색해야하므로 여러개 > 배열
    public Transform nearestTarget;


    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector3.zero , 0 , targetLayer); // 원형으로 레이를 캐스팅하여 그 결과 전부를 반환
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;

        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos,targetPos);

            if(curDiff < diff)// 더 작은 거리로 result 변경
            {
                diff = curDiff;
                result = target.transform;
            }
        }


        return result;
    }


}
