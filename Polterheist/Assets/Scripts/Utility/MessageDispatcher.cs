using System;
using System.Collections.Generic;

public static class MessageDispatcher {
    private static Dictionary<string, Action<Message>> listeners = new Dictionary<string, Action<Message>>();

    public static void AddListener(string messageType, Action<Message> listener) {
        if (!listeners.ContainsKey(messageType)) {
            listeners.Add(messageType, null);
        }
        listeners[messageType] += listener;
    }

    public static void RemoveListener(string messageType, Action<Message> listener) {
        if (listeners.ContainsKey(messageType)) {
            listeners[messageType] -= listener;
        }
    }

    public static void SendMessage(Message message) {
        if (listeners.ContainsKey(message.MessageType)) {
            listeners[message.MessageType]?.Invoke(message);
        }
    }
}

public class Message {
    public string MessageType;
    public object Data;

    public Message(string type, object data) {
        MessageType = type;
        Data = data;
    }
}

//EXAMPLE!
//
//
// This is entered in a script sending a message out. in this case, a controller script sending a message to the world that the how to play panel is being toggled
//bool toggled;
//public void ToggleHowToPlay() {
//    toggled = !toggled;
//    Message message = new Message(GlobalNames.UI.SHOW_HOW_TO_PLAY, toggled);
//    MessageDispatcher.SendMessage(message);
//}

// This is entered in a separate script that is listening for the message. so for example, in a UI manager script we would want to know when we are requesting the how to play panel to be shown
//public void AddListeners() {
//    MessageDispatcher.AddListener(GlobalNames.UI.SHOW_HOW_TO_PLAY, ToggleHowToPlayListener);
//}

//public void RemoveListeners() {
//    MessageDispatcher.RemoveListener(GlobalNames.UI.SHOW_HOW_TO_PLAY, ToggleHowToPlayListener);
//}

//public void ToggleHowToPlayListener(Message message) {
//    if (message.Data is bool whatodo) {
//        Debug.Log(whatodo);
//    }
//}