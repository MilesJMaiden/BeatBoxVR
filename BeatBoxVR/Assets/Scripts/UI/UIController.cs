using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public GameObject drumKit;
    public float animationDuration = 1.0f;
    public CanvasGroup mainMenuCanvasGroup;

    public void ToggleMenu(bool show)
    {
        //StartCoroutine(AnimateDrumKit(show));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, show ? 1 : 0, animationDuration));
    }

    //private IEnumerator AnimateDrumKit(bool show)
    //{
    //    Vector3 targetPosition = show ? new Vector3(0, 0, 1) : Vector3.zero; // Adjust target position
    //    float elapsedTime = 0;

    //    while (elapsedTime < animationDuration)
    //    {
    //        drumKit.transform.localPosition = Vector3.Lerp(drumKit.transform.localPosition, targetPosition, elapsedTime / animationDuration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Additional methods for UI interactions...
}
