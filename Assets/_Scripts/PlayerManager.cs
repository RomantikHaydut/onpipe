using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject theRing;

    public const float sizeFactor = 0.28f;

    public float SizeFactor { get { return sizeFactor; } }

    [SerializeField] Vector3 sizeDefault;

    private float startZScale;

    private float sizeChangeTime = 0.15f;

    [SerializeField] LayerMask cylinderLayer;

    public float interactRadius = 0.18f;

    public float enemyOffset;

    [HideInInspector] public GameObject radiusPoint;

    public float radius;

    public bool canCollect;

    public GameObject activeCylinder;

    public float health;

    public float score;

    private float startZPosition;

    private UIManager uiManager;

    private MeshRenderer meshRenderer;

    private bool isPressingSpeedButton;
    #region Colors
    Color startColor;

    public Color yellowColor;
    public Color blackColor;
    public Color collectorColor;

    Color activeColor;
    #endregion
    #region Sounds
    private AudioSource audioSource;

    [SerializeField] AudioClip smallerSound;
    [SerializeField] AudioClip biggerSound;
    public AudioSource orcSound;
    public AudioSource mainSoundtrack;
    #endregion
    #region Materials
    public Material yellowMaterial;
    public Material blackMaterial;
    #endregion
    private void Awake()
    {
        meshRenderer = theRing.GetComponent<MeshRenderer>();

        if (DataManager.Instance.isRingBlack)
        {
            meshRenderer.material = blackMaterial;
            collectorColor = yellowColor;
        }
        else if (DataManager.Instance.isRingYellow)
        {
            meshRenderer.material = yellowMaterial;
            collectorColor = blackColor;
        }
        else
        {
            meshRenderer.material = yellowMaterial;
            collectorColor = blackColor;
        }


        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1;

        sizeDefault = transform.localScale;

        radiusPoint = transform.GetChild(0).gameObject;

        health = 100;

        uiManager = FindObjectOfType<UIManager>();

        startZPosition = transform.position.z;

        startZScale = transform.localScale.z;
    }

    private void Start()
    {
        startColor = theRing.GetComponent<MeshRenderer>().material.color;
        activeColor = startColor;
    }

    void Update()
    {
        Collider[] Cylinders = Physics.OverlapSphere(transform.position, interactRadius, cylinderLayer);

        if (Cylinders.Length >= 1 && Cylinders != null)
        {
            activeCylinder = Cylinders[0].gameObject;
        }


        Vector3 cylinderScales = Cylinders[0].gameObject.transform.localScale * sizeFactor;

        CheckDeath(cylinderScales, Cylinders);

        ChangeRingSize(cylinderScales);

        radius = Vector3.Distance(radiusPoint.transform.position, transform.position);

        HealthCounter();

        CalculateScore();

    }


    private void CheckDeath(Vector3 cylinderScales, Collider[] Cylinders)
    {
        if (cylinderScales.z > transform.localScale.x)
        {
            GameOver();
        }

        if (Cylinders[0].gameObject.CompareTag("Enemy"))
        {
            if (cylinderScales.z + enemyOffset > transform.localScale.x)
            {
                GameOver();
            }

        }
    }

    private void ChangeRingSize(Vector3 cylinderScales)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            print(touch.position);
            if (touch.position.x > 700 && touch.position.y < 700)
            {
                isPressingSpeedButton = true;
                
            }
            else
            {
                isPressingSpeedButton = false;
            }

            if (!isPressingSpeedButton)
            {
                float targetZScale = (cylinderScales.z / transform.localScale.x) * transform.localScale.z;
                Vector3 targetVector = new Vector3(cylinderScales.z, cylinderScales.x, targetZScale);

                if (touch.phase == TouchPhase.Stationary)
                {
                    transform.localScale = Vector3.Slerp(transform.localScale, targetVector, sizeChangeTime);
                    activeColor = Color.Lerp(activeColor, collectorColor, 0.3f);
                    theRing.GetComponent<MeshRenderer>().material.color = activeColor;
                }


                // Sounds..
                if (touch.phase == TouchPhase.Began)
                {
                    PlaySmallerSound();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    PlayBiggerSound();
                }
            }
            
        }
        else
        {
            transform.localScale = Vector3.Slerp(transform.localScale, sizeDefault, sizeChangeTime);
            activeColor = Color.Lerp(activeColor, startColor, 0.3f);
            theRing.GetComponent<MeshRenderer>().material.color = activeColor;
        }


        // Check can we collect points.
        if (Mathf.Abs(cylinderScales.z - transform.localScale.x) <= 0.05f)
        {
            canCollect = true;
        }
        else
        {
            canCollect = false;
        }
    }

    void CalculateScore()
    {
        score = transform.position.z - startZPosition;
        uiManager.AddScore((int)score);
    }

    private void HealthCounter()
    {
        if (health >= 0)
        {
            health -= Time.deltaTime * 5;
            uiManager.ControlHealthBar(health);
        }
        else
        {
            GameOver();
        }

    }

    public void AddHealth(float amount)
    {
        if (health <= 100)
        {
            health += amount;
        }

        if (health > 100)
        {
            health = 100;
        }

    }

    private void GameOver()
    {
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null)
        {
            camController.enabled = false;
            GameManager.Instance.isPlayerAlive = false;

            StartCoroutine(MainSoundtrackFinish());
            DataManager.Instance.SaveBestScore((int)score);
            uiManager.OpenGameOverPanel();
            this.enabled = false;
        }
    }

    IEnumerator MainSoundtrackFinish()
    {
        while (true)
        {
            yield return null;
            if (mainSoundtrack.volume <= 0.05f)
            {
                mainSoundtrack.volume = 0;
                yield break;
            }
            mainSoundtrack.volume -= 0.14f * Time.deltaTime;
        }
    }

    void PlaySmallerSound()
    {

        if (audioSource.isPlaying && audioSource.clip.name != "Smaller")
        {
            audioSource.Stop();
        }
        audioSource.clip = smallerSound;
        audioSource.Play();
    }

    void PlayBiggerSound()
    {
        if (audioSource.isPlaying && audioSource.clip.name != "Bigger")
        {
            audioSource.Stop();
        }
        audioSource.clip = biggerSound;
        audioSource.Play();
    }

    public void OrcSound()
    {
        if (orcSound.isPlaying)
        {
            orcSound.Stop();
        }
        orcSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cylinder"))
        {
            StartCoroutine(InactiveCylinder(other.gameObject));
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(InactiveCylinder(other.gameObject));
        }
    }
    IEnumerator InactiveCylinder(GameObject cylinder_)
    {
        yield return new WaitForSecondsRealtime(4f);
        if (!GameManager.Instance.isPlayerAlive)
        {
            yield break;
        }
        cylinder_.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, interactRadius);
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveBestScore((int)score);
    }

}
