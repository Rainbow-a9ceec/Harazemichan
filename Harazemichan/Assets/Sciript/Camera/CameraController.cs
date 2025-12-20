using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    private Vector3 offset;

    [Header("Zoom Settings")]
    [SerializeField] float baseFOV = 60f;           // 基準の高さ時のFOV
    [SerializeField] float maxFOV = 90f;            // 最大ズームアウトFOV
    [SerializeField] float minFOV = 30f;            // 最小ズームインFOV（オプション）
    [SerializeField] float heightMultiplier = 10f;  // Y高さ1あたりFOV増加量
    [SerializeField] float lerpSpeed = 2f;          // スムーズさ

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Cameraコンポーネントがありません");
            return;
        }

        offset = transform.position - playerObj.transform.position;
    }

    void LateUpdate()
    {
        if (playerObj == null) return;

        // プレイヤー位置追従（Y軸はoffsetで固定）
        Vector3 targetPos = playerObj.transform.position + offset;
        transform.position = targetPos;

        // Y位置に応じたFOV計算
        float playerHeight = playerObj.transform.position.y;
        float targetFOV = baseFOV + (playerHeight * heightMultiplier);
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);

        // スムーズにFOV変更（ズームアウト）
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, lerpSpeed * Time.deltaTime);
    }
}

