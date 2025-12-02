using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;
    
    private bool isFading = false;
    private CanvasGroup fadeCanvasGroup;
    
    void Awake()
    {
        // Singleton pattern that persists between scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Make sure fade image exists
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
            
            // Get or add CanvasGroup for controlling raycasts
            fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (fadeCanvasGroup == null)
            {
                fadeCanvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();
            }
            
            // Start with fade in from black
            StartCoroutine(FadeIn());
        }
    }
    
    /// <summary>
    /// Fade to black then load scene
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }
    }
    
    /// <summary>
    /// Fade to black then load scene by index
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        if (!isFading)
        {
            StartCoroutine(TransitionToScene(sceneIndex));
        }
    }
    
    /// <summary>
    /// Reload current scene with fade
    /// </summary>
    public void ReloadCurrentScene()
    {
        if (!isFading)
        {
            StartCoroutine(TransitionToScene(SceneManager.GetActiveScene().buildIndex));
        }
    }
    
    IEnumerator TransitionToScene(string sceneName)
    {
        isFading = true;
        
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Load scene
        SceneManager.LoadScene(sceneName);
        
        // Wait one frame for scene to load
        yield return null;
        
        // Find the FadeImage in the new scene
        FindFadeImageInScene();
        
        // Fade in
        yield return StartCoroutine(FadeIn());
        
        isFading = false;
    }
    
    IEnumerator TransitionToScene(int sceneIndex)
    {
        isFading = true;
        
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Load scene
        SceneManager.LoadScene(sceneIndex);
        
        // Wait one frame for scene to load
        yield return null;
        
        // Find the FadeImage in the new scene
        FindFadeImageInScene();
        
        // Fade in
        yield return StartCoroutine(FadeIn());
        
        isFading = false;
    }
    
    void FindFadeImageInScene()
    {
        // Find all images in the scene
        Image[] images = FindObjectsOfType<Image>();
        
        // Look for one named "FadeImage"
        foreach (Image img in images)
        {
            if (img.gameObject.name == "FadeImage")
            {
                fadeImage = img;
                
                // Get or add CanvasGroup
                fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>();
                if (fadeCanvasGroup == null)
                {
                    fadeCanvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();
                }
                
                break;
            }
        }
    }
    
    IEnumerator FadeOut()
    {
        if (fadeImage == null) 
        {
            Debug.LogWarning("FadeImage is null during FadeOut");
            yield break;
        }
        
        // Block raycasts during fade
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
        }
        
        float elapsedTime = 0f;
        Color color = fadeColor;
        color.a = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            
            if (fadeImage != null)
            {
                fadeImage.color = color;
            }
            else
            {
                yield break;
            }
            
            yield return null;
        }
        
        if (fadeImage != null)
        {
            color.a = 1f;
            fadeImage.color = color;
        }
    }
    
    IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeImage is null during FadeIn");
            yield break;
        }
        
        // Block raycasts at start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
        }
        
        float elapsedTime = 0f;
        Color color = fadeColor;
        color.a = 1f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            
            if (fadeImage != null)
            {
                fadeImage.color = color;
            }
            else
            {
                yield break;
            }
            
            yield return null;
        }
        
        if (fadeImage != null)
        {
            color.a = 0f;
            fadeImage.color = color;
        }
        
        // Allow clicks after fade is complete
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }
    
    public bool IsFading()
    {
        return isFading;
    }
}