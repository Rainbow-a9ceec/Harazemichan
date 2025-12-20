using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadValue : MonoBehaviour
{
    [Header("振りの閾値")]
    [SerializeField, Range(0, 65534)] int shakeThreshold=60000;
    [Header("振る時間")]
    [SerializeField] int ShakeTime;
    [SerializeField] float angleMultiplier = 30f;  // 傾きを回転角に変換する倍率
    [SerializeField] float lerpSpeed = 5f;        // スムーズさ
    [SerializeField] float speed = 30f;
    [Header("必要なコンポーネント")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject button;
    public int value { private get; set; }  //加速度センサーの値

    public int value2 { private get; set; }

    public int swValue {  private get; set; }   //スイッチの値

    int swCount;

    int beforeValue;

    public int shakeCount { private set; get; } //缶を振った回数

    private float currentAngle = 0f;
    private float lastTime = 0f;
    private const float GYRO_SCALE = 131.0f;  // MPU6050 ±250°/s

    float ShakeTimer;

    public bool isChecking { private set; get; }    //振り開始フラグ

    public bool endShake { private set; get; }      //振り終了フラグ
    public bool decideAngle;  //角度決定フラグ.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = 0;
        value2 = 0;
        swValue = 1;
        swCount = 0;
        beforeValue = 0;
        shakeCount = 0;
        ShakeTimer = 0;
        //countText.text=shakeCount.ToString();
        timerText.text=((int)(ShakeTime-ShakeTimer)).ToString();
        isChecking = false;
        endShake = false;
        decideAngle = false;
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecking)
        {

            if (!endShake)
            {


                if (CheckValue())
                {
                    shakeCount++;
                    countText.text = shakeCount.ToString();
                }



                ShakeTimer += Time.deltaTime;
                timerText.text = ((int)(ShakeTime - ShakeTimer)).ToString();
                if (ShakeTimer >= ShakeTime)
                    endShake = true;
            }
            else if(endShake&&!decideAngle)
            {
                ChangeAngleZ();
                if (swValue == 0)
                {
                    swCount++;
                    //if (swCount > 5)
                    {
                        decideAngle = true;
                        isChecking = false;
                        endShake = false; 
                        Debug.Log("角度を決定しました。");
                        //decideAngle = false;
                    }
                    
                }
            }
        }
    }

    public void CheckStart()
    {
        isChecking = true;
        button.gameObject.SetActive(false); 
    }

    public bool CheckValue()
    {
        //Debug.Log(value);
        int diff= Mathf.Abs(beforeValue-value);
        Debug.Log("diff=" + diff);
        beforeValue= value; 
        //var current=Keyboard.current;

        //if(current.spaceKey.wasPressedThisFrame)
        //{
        //    return true;
        //}

        if (diff >= shakeThreshold)
        {
            shakeCount++;
            countText.text = shakeCount.ToString();
            return true;
        }

        return false;
            
    }

    void ChangeAngleZ()
    {
        float dt = Time.time - lastTime;
        if (dt > 0.001f)
        {
            float angularVelocity = value / GYRO_SCALE;
            currentAngle += angularVelocity * dt;
            currentAngle = Mathf.Clamp(currentAngle, -180, 180);
        }

        lastTime = Time.time;

        this.transform.rotation = Quaternion.Euler(0, -180, -currentAngle);

        float targetZ = value * angleMultiplier;

        //// 現在の角度を取得（-180〜180 に正規化）
        //float currentZ = transform.eulerAngles.z;
        //if (currentZ > 180f) currentZ -= 360f;

        //// 補間して滑らかに追従
        //float newZ = Mathf.Lerp(currentZ, targetZ, lerpSpeed * Time.deltaTime);

        //// z だけ変更して適用
        //Vector3 euler = transform.eulerAngles;
        //euler.z = newZ;
        //transform.eulerAngles = euler;

        //var dire = Vector3.zero;

        //dire.z = value ;
        //dire.x = value2;

        //float direZ = value;
        //direZ*=Time.deltaTime;

        //transform.Rotate(0,-180,direZ);

        //dire *= Time.deltaTime;
        //transform.Rotate(dire * speed);
    }

    void IncrementShakeCount()
    {
        shakeCount++;
        countText.text=shakeCount.ToString();
    }
}
