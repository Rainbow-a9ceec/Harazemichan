using System.Runtime.Remoting.Messaging;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadValue : MonoBehaviour
{
    [Header("振りの閾値")]
    [SerializeField, Range(0, 65534)] int shakeThreshold=60000;
    [Header("振る時間")]
    [SerializeField] int ShakeTime;
    [Header("必要なコンポーネント")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI timerText;
    public int value { private get; set; }  //加速度センサーの値

    public int swValue {  private get; set; }   //スイッチの値

    int beforeValue;

    public int shakeCount { private set; get; } //缶を振った回数

    private float currentAngle = 0f;
    private float lastTime = 0f;
    private const float GYRO_SCALE = 131.0f;  // MPU6050 ±250°/s

    float ShakeTimer;

    public bool isChecking { private set; get; }

    public bool endShake { private set; get; }
    bool decideAngle;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = 0;
        swValue = 0;
        beforeValue = 0;
        shakeCount = 0;
        ShakeTimer = 0;
        countText.text=shakeCount.ToString();
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
                if(swValue==1)
                    decideAngle=true;
            }
        }
    }

    public void CheckStart()
    {
        isChecking = true;
    }

    bool CheckValue()
    {
        Debug.Log(value);
        int diff= Mathf.Abs(beforeValue-value);
        Debug.Log("diff=" + diff);
        beforeValue= value;
        if(diff>shakeThreshold)
            return true;
        else
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

        this.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void IncrementShakeCount()
    {
        shakeCount++;
        countText.text=shakeCount.ToString();
    }
}
