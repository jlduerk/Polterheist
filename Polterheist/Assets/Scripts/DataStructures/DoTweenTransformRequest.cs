using UnityEngine;
using DG.Tweening;

/// <summary>
/// A generic way to call DoTweens for a Transform component!
/// </summary>
public class DoTweenTransformRequest : MonoBehaviour {
    public enum TransformType {
        Move = 0, // DOMove()
        Rotate = 1, // DORotate()
        Scale = 2, // DoScale()
    }
    public TransformType transformType;

    public Transform tweenTransform;
    public Vector3 moveTo;
    public float duration;
    public Ease easeType = Ease.Linear;
    public float delay; // start delay

    /// <summary>
    /// Creates a DoTween for the tweenTarget's transform with the settings
    /// </summary>
    /// <param name="tweenTransformOverride">the transform to override current tweenTarget</param>
    public void Play(Transform tweenTransformOverride = null) {
        if (tweenTransformOverride) { // if a script requested an override, set new tweenTarget
            tweenTransform = tweenTransformOverride;
        }
        
        Invoke("DoRequest", delay);
    }

    public void DoRequest() {
        switch (transformType) {
            case TransformType.Move:
                tweenTransform.DOMove(moveTo, duration).SetEase(easeType);
                break;
            case TransformType.Rotate:
                tweenTransform.DORotate(moveTo, duration).SetEase(easeType);
                break;
            case TransformType.Scale:
                tweenTransform.DOScale(moveTo, duration).SetEase(easeType);
                break;
        }
    }
}