using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MouseUI : MonoBehaviour
{
    public RectTransform pointerImage;
    public float inactivityDuration = 5f;

    private Vector3 lastMousePosition;
    private float lastMoveTime;

    private static MouseUI instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.visible = false;

        lastMousePosition = Mouse.current.position.ReadValue();
        lastMoveTime = Time.unscaledTime;
        pointerImage.gameObject.SetActive(true);

        UpdatePointerPositionImmediately();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePointerPositionImmediately();
    }

    private void Update()
    {
        Vector2 currentMousePos = Mouse.current.position.ReadValue();

        if (Vector2.Distance(currentMousePos, lastMousePosition) > 0.1f)
        {
            pointerImage.position = currentMousePos;
            lastMousePosition = currentMousePos;
            lastMoveTime = Time.unscaledTime;

            if (!pointerImage.gameObject.activeSelf)
                pointerImage.gameObject.SetActive(true);
        }
        else
        {
            if (Time.unscaledTime - lastMoveTime >= inactivityDuration)
            {
                if (pointerImage.gameObject.activeSelf)
                    pointerImage.gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePointerPositionImmediately()
    {
        Vector2 currentMousePos = Mouse.current.position.ReadValue();
        pointerImage.position = currentMousePos;
    }
}
