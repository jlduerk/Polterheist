using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BobTransform : MonoBehaviour
{

    //private Image image;

    public Vector2 movementTarget = new(0, 20);
    public float duration = 1f;
    public bool snapping = false;

    // Start is called before the first frame update
    void Start()
    {

        Vector3 movementTarget3D = new(transform.position.x + movementTarget.x, transform.position.y + movementTarget.y);

        transform.DOMove(movementTarget3D, duration, snapping).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}