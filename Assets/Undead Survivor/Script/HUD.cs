using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
   public enum InfoType
    {
        Exp, // 경험치 - 올라가야
        Level, // 레벨 - 올라가야
        Kill, // 킬 수
        Time, // 시간 - 줄어들어야
        Health // 체력
    }

    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                //myText.text = GameManager.instance.level.ToString();
                //myText.text = string.Format($"Lv. {GameManager.instance.level}");
                myText.text = string.Format("Lv. {0:F0}",GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // 분 계산 > 몫 계산
                int sec = Mathf.FloorToInt(remainTime % 60); // 초 계산 > 나머지 계산
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
            default:
                break;
        }
    }





}
