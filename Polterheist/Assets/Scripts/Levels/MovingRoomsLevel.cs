using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MovingRoomsLevel : MonoBehaviour
{
    [SerializeField] private float roomMoveInterval = 10.0f;

    [SerializeField] private Rigidbody redRoomRB = null;
    [SerializeField] private Rigidbody blueRoomRB = null;

    [SerializeField] private Transform roomAttachPointsParent = null;

    private int redRoomAttachPointIdx = 1;
    private int blueRoomAttachPointIdx = 4;

    private float redRoomShuffleHeight = -10.0f;
    private float blueRoomShuffleHeight = -5.0f;

    private float timeUntilNextShuffle = 0.0f;

    private void Start()
    {
        timeUntilNextShuffle = roomMoveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextShuffle -= Time.deltaTime;
        if (timeUntilNextShuffle <= 0.0f)
        {
            timeUntilNextShuffle = roomMoveInterval;
            SlideRooms();
        }
    }

    private void ShuffleRooms()
    {
        int newRedAttachPointIdx;
        do
        {
            newRedAttachPointIdx = Random.Range(0, roomAttachPointsParent.childCount);
        }
        while (newRedAttachPointIdx == redRoomAttachPointIdx);

        int newBlueAttachPointIdx;
        do
        {
            newBlueAttachPointIdx = Random.Range(0, roomAttachPointsParent.childCount);
        }
        while (newBlueAttachPointIdx == blueRoomAttachPointIdx || newBlueAttachPointIdx == newRedAttachPointIdx);

        redRoomAttachPointIdx = newRedAttachPointIdx;
        blueRoomAttachPointIdx = newBlueAttachPointIdx;

        Sequence shuffleSequence = DOTween.Sequence();

        shuffleSequence.Insert(0.0f, redRoomRB.DOMoveY(redRoomShuffleHeight, 2.0f));
        Vector3 intermediateRedPoint = GetCurrentAttachPoint(true).position;
        intermediateRedPoint.y = redRoomShuffleHeight;
        shuffleSequence.Insert(2.0f, redRoomRB.DOMove(intermediateRedPoint, 2.0f));
        shuffleSequence.Insert(2.0f, redRoomRB.DORotate(GetCurrentAttachPoint(true).eulerAngles, 1.0f));
        shuffleSequence.Insert(4.0f, redRoomRB.DOMoveY(0.0f, 2.0f));

        shuffleSequence.Insert(0.0f, blueRoomRB.DOMoveY(blueRoomShuffleHeight, 2.0f));
        Vector3 intermediateBluePoint = GetCurrentAttachPoint(false).position;
        intermediateBluePoint.y = blueRoomShuffleHeight;
        shuffleSequence.Insert(2.0f, blueRoomRB.DOMove(intermediateBluePoint, 2.0f));
        shuffleSequence.Insert(2.0f, blueRoomRB.DORotate(GetCurrentAttachPoint(false).eulerAngles, 1.0f));
        shuffleSequence.Insert(4.0f, blueRoomRB.DOMoveY(0.0f, 2.0f));

        shuffleSequence.Play();
    }

    // Just slide without rotating or rising/falling
    private void SlideRooms()
    {
        int newRedAttachPointIdx;
        do
        {
            newRedAttachPointIdx = Random.Range(0, roomAttachPointsParent.childCount / 2);
        }
        while (newRedAttachPointIdx == redRoomAttachPointIdx);

        int newBlueAttachPointIdx;
        do
        {
            newBlueAttachPointIdx = Random.Range(roomAttachPointsParent.childCount / 2, roomAttachPointsParent.childCount);
        }
        while (newBlueAttachPointIdx == blueRoomAttachPointIdx);

        int redIdxDiff = Mathf.Abs(redRoomAttachPointIdx - newRedAttachPointIdx);
        int blueIdxDiff = Mathf.Abs(blueRoomAttachPointIdx - newBlueAttachPointIdx);

        redRoomAttachPointIdx = newRedAttachPointIdx;
        blueRoomAttachPointIdx = newBlueAttachPointIdx;

        redRoomRB.DOMove(GetCurrentAttachPoint(true).position, redIdxDiff * 1.0f);
        blueRoomRB.DOMove(GetCurrentAttachPoint(false).position, blueIdxDiff * 1.0f);
    }

    private Transform GetCurrentAttachPoint(bool forRedRoom)
    {
        int attachPointIdx = forRedRoom ? redRoomAttachPointIdx : blueRoomAttachPointIdx;

        if (attachPointIdx < 0 || attachPointIdx >= roomAttachPointsParent.childCount)
            return null;

        return roomAttachPointsParent.GetChild(attachPointIdx);
    }
}
