using UnityEngine;

public class Follow : MonoBehaviour
{
   RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    private void FixedUpdate()
    {
        //rect.position = GameManager.instance.player.transform.position; //  이렇게 하면 안된다.
        //UI좌표계와 게임 월드 좌표계는 아예 다른 곳이기 때문
        if (rect != null) 
        {
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }
}
