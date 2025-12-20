using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchPlayer : MonoBehaviour
{

    [SerializeField] int power = 1000;
    [SerializeField] GameObject RestartButton;
    [SerializeField] GameObject EndButton;
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    public float stopThreshold = 0.1f; // ���x臒l
    private bool isLanded = false;

    LoadValue loadValue;
    Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadValue = this.GetComponent<LoadValue>();
        rb = this.GetComponent<Rigidbody>();

        RestartButton.SetActive(false);
        EndButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (loadValue.decideAngle)
        {
            rb.useGravity = true;
            rb.AddForce(this.transform.up * loadValue.shakeCount * power);
            loadValue.decideAngle = false;
        }
    }

    void FixedUpdate()
    {
        if (isLanded)
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed < stopThreshold || rb.IsSleeping())
            {
                RestartButton.SetActive(true);
                EndButton.SetActive(true);


                Debug.Log("���S�Ɏ~�܂�܂���");
                // �����Ŏ~�܂菈���i��: �A�j���[�V�����ύX�j
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            // ���n���m: �����Œ��n�C�x���g�𔭉�
            RestartButton.SetActive(true);
            EndButton.SetActive(true);
            Debug.Log("���n���܂���");
            // �~�܂�m�F�ֈڍs
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
}
