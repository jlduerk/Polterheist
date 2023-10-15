using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform doorA = null;
    [SerializeField] private Transform doorB = null;
    [SerializeField] private Collider doorBlocker = null;
    [SerializeField] private float doorOpenAmt = 100.0f;
    [SerializeField] private float openDuration = 0.5f;
    [SerializeField] private Ease openEasing;
    [SerializeField] private float closeDuration = 0.5f;
    [SerializeField] private Ease closeEasing;


    public void OpenDoors()
    {
        doorA.DOLocalRotate(Vector3.down * doorOpenAmt, openDuration).SetEase(openEasing);
        doorB.DOLocalRotate(Vector3.up * doorOpenAmt, openDuration).SetEase(openEasing);
        doorBlocker.enabled = false;
    }

    public void CloseDoors()
    {
        doorA.DOLocalRotate(Vector3.zero, closeDuration).SetEase(closeEasing);
        doorB.DOLocalRotate(Vector3.zero, closeDuration).SetEase(closeEasing);
        doorBlocker.enabled = true;
    }
}
