using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelSelectNotificationPanel : MonoBehaviour {
    public TMP_Text notificationTMP;
    public float fadeDuration = 1;
    public float announcementFadeDelay = 1;
    public Ease fadeEase;

    //this part is hard-coded and more one off-y. if we work on this again this will def need changes lol
    private const int NUM_LEVELS = 3;
    private int currentLevelIndex;
    private string currentLevelName;

    private void Start() {
        AddListeners();
    }

    private void OnDisable() {
        RemoveListeners();
    }

    private void NewAnnouncement(string announcementText) {
        notificationTMP.text = announcementText;
        DOTween.Sequence()
           .Append(notificationTMP.DOFade(1, fadeDuration).SetEase(fadeEase))
           .AppendInterval(announcementFadeDelay) // Change waitDuration to the time you want to wait before fading out
           .Append(notificationTMP.DOFade(0, fadeDuration).SetEase(fadeEase));
    }

    private void NewAnnouncementListener(Message message) {
        if (message.Data is string announcementText) {
            NewAnnouncement(announcementText);
        }
    }

    private void AddListeners() {
        MessageDispatcher.AddListener(GlobalNames.UI.ANNOUNCEMENT, NewAnnouncementListener);
    }

    private void RemoveListeners() {
        MessageDispatcher.RemoveListener(GlobalNames.UI.ANNOUNCEMENT, NewAnnouncementListener);
    }


    //this part is hard-coded and more one off-y. if we work on this again this will def need changes lol
    public void IncrementCurrentLevel() {
        currentLevelIndex++;
        if (currentLevelIndex >= NUM_LEVELS) {
            currentLevelIndex = 0;
        }
        switch (currentLevelIndex) {
            case 0:
                currentLevelName = "Mansi0n";
                break;
            case 1:
                currentLevelName = "H0tel";
                break;
            case 2:
                currentLevelName = "Basement";
                break;
        }
        string announceLevelText = $"Current Level: {currentLevelName}";
        Message message = new Message(GlobalNames.UI.ANNOUNCEMENT, announceLevelText);
        MessageDispatcher.SendMessage(message);
        //LoadCurrrentSelectedLevel();
    }
    //this part is hard-coded and more one off-y. if we work on this again this will def need changes lol
    public void LoadCurrrentSelectedLevel() {
        //need to convert it back cus we make our O's into 0's due to the font making O's ghosts lol
        switch (currentLevelIndex) {
            case 0:
                currentLevelName = "Mansion";
                break;
            case 1:
                currentLevelName = "Hotel";
                break;
            case 2:
                currentLevelName = "Basement";
                break;
        }
        UICommands.LoadScene(currentLevelName);
    }
}