using UnityEngine;
using DG.Tweening;

/// <summary>
/// A generic way to call DoTweens!
/// </summary>
public class DoTweenRequest : MonoBehaviour {
   
    public enum DoTweenRequestType {
        Float = 0, //DOTo()
        Move = 1, // DOMove()
        Rotate = 2, // DORotate()
        Scale = 3, // DoScale()
    }
    public DoTweenRequestType doTweenRequestType;

    public Transform tweenTarget;
    public float from; // optional. used for Float Tweens
    public float to;
    public Vector3 moveTo;
    public float duration;
    public Ease easeType = Ease.Linear;

    /// <summary>
    /// Creates a DoTween with the settings
    /// </summary>
    /// <param name="tweenTargetOverride">the transform to override current tweenTarget</param>
    public void Play(Transform tweenTargetOverride = null) {
        if (tweenTargetOverride) { // if a script requested an override, set new tweenTarget
            tweenTarget = tweenTargetOverride;
        }
        
        switch (doTweenRequestType) {
            case DoTweenRequestType.Float:
                DOTween.To(()=> from, x=> from = x, to, duration).SetEase(easeType);
                break;
            case DoTweenRequestType.Move:
                tweenTarget.DOMove(moveTo, duration).SetEase(easeType);
                break;
            case DoTweenRequestType.Rotate:
                tweenTarget.DORotate(moveTo, duration).SetEase(easeType);
                break;
            case DoTweenRequestType.Scale:
                tweenTarget.DOScale(moveTo, duration).SetEase(easeType);
                break;
        }
    }
    
    //TODO? sequences
}