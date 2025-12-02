using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class PhotoMode : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button photoButton;
    [SerializeField] private Text feedbackText; // Optional - shows "Screenshot saved!"
    
    [Header("Settings")]
    [SerializeField] private string screenshotFolderName = "Screenshots";
    [SerializeField] private float feedbackDuration = 2f;
    
    private string screenshotPath;
    
    void Start()
    {
        // Setup screenshot folder path
        screenshotPath = Path.Combine(Application.persistentDataPath, screenshotFolderName);
        
        // Create folder if it doesn't exist
        if (!Directory.Exists(screenshotPath))
        {
            Directory.CreateDirectory(screenshotPath);
        }
        
        // Setup button listener
        if (photoButton != null)
        {
            photoButton.onClick.AddListener(TakeScreenshot);
        }
        
        // Hide feedback text initially
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
        
        Debug.Log($"Screenshots will be saved to: {screenshotPath}");
    }
    
    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshot());
    }
    
    IEnumerator CaptureScreenshot()
    {
        // Generate unique filename with timestamp
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = $"ZomBrawl_Screenshot_{timestamp}.png";
        string fullPath = Path.Combine(screenshotPath, filename);
        
        // Wait for end of frame to capture
        yield return new WaitForEndOfFrame();
        
        // Capture screenshot
        ScreenCapture.CaptureScreenshot(fullPath);
        
        Debug.Log($"Screenshot saved to: {fullPath}");
        
        // Show feedback message
        if (feedbackText != null)
        {
            StartCoroutine(ShowFeedback($"Screenshot saved!\n{filename}"));
        }
        
        // Optional: Open folder (uncomment if you want)
        // Application.OpenURL("file://" + screenshotPath);
    }
    
    IEnumerator ShowFeedback(string message)
    {
        if (feedbackText == null) yield break;
        
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(feedbackDuration);
        
        feedbackText.gameObject.SetActive(false);
    }
    
    // Optional: Call this to open the screenshots folder
    public void OpenScreenshotFolder()
    {
        Application.OpenURL("file://" + screenshotPath);
    }
}