using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public GameObject drumKit;
    public float animationDuration = 1.0f;
    public CanvasGroup mainMenuCanvasGroup;

    // Function to toggle the main menu visibility
    public void ToggleMenu(bool show)
    {
        StopAllCoroutines();

        //When mainMenuCanvasGroup show value is true, activate.
        if (show)
        {
            mainMenuCanvasGroup.gameObject.SetActive(true);
            mainMenuCanvasGroup.alpha = 1;
        }
        else
        {
            // 
            StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, show ? 1 : 0, animationDuration));
        }

        StartCoroutine(AnimateDrumKit(show));
        
    }

    private IEnumerator AnimateDrumKit(bool show)
    {
        Vector3 targetPosition = show ? new Vector3(0, 0, 10) : Vector3.zero; // Adjust target position
        float elapsedTime = 0;

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.02f);
        while (elapsedTime < animationDuration)
        {
            drumKit.transform.localPosition = Vector3.Lerp(drumKit.transform.localPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += 0.02f;
            yield return wait;
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0;

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.02f);
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += 0.02f;
            yield return wait;
        }

        mainMenuCanvasGroup.gameObject.SetActive(false);
    }

    // Additional methods for UI interactions...
}
