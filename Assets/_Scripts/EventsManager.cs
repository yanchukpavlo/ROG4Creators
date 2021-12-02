using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<bool> onGameStart;
    public void GameStart(bool isStart)
    {
        onGameStart?.Invoke(isStart);
    }
}
