using UnityEngine;
using DG.Tweening;

public class ToggleScaler : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private float fadeDuration = 1f;

    private bool isScaled = false;

    public void Awake()
    {
        targetRectTransform.DOScale(Vector3.zero, 0).SetUpdate(true).Play();
    }

    public void ToggleScale()
    {
        if (isScaled)
        {
            targetRectTransform.DOScale(Vector3.zero, fadeDuration).SetUpdate(true).Play();
        }
        else
        {
            targetRectTransform.DOScale(Vector3.one, fadeDuration).SetUpdate(true).Play();
        }

        isScaled = !isScaled;
    }
}