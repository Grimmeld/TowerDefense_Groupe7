using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
public static GameManager Instance;

int currentLevel = 0;
    GameSate currentState;
    public event Action<GameSate> OnStateChanged;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if(keyboard.spaceKey.wasPressedThisFrame)
        {
            ChangeState(GameSate.CardSelection);
            currentLevel++;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void ChangeState(GameSate newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(newState);
        HandleStateChange();
    }

    private void HandleStateChange()
    {
        switch(currentState)
        {
            case GameSate.Playing:
                CardManager.Instance.HideCardSelection();
                break;
            case GameSate.CardSelection:
                CardManager.Instance.ShowCardSelection();
                break;
        }
    }

    public enum GameSate
    {
        Playing,
        CardSelection
    }

}
