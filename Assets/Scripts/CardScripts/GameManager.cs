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
    [SerializeField] private GameObject CardTitle;

    private void Update() //Test des cartes
    {
        var keyboard = Keyboard.current;
        if(keyboard.spaceKey.wasPressedThisFrame)
        {

            ChangeState(GameSate.CardSelection);
            currentLevel++;
        }
        if(currentState == GameSate.CardSelection)
            CardTitle.SetActive(true);
        else
            CardTitle.SetActive(false);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void ChangeState(GameSate newState) //Changer l'état du jeu (combat / selection de carte)
    {
        Debug.Log("state change");
        currentState = newState;
        OnStateChanged?.Invoke(newState);
        HandleStateChange();
    }

    private void HandleStateChange() // les types d'état de jeu
    {
        switch(currentState)
        {
            case GameSate.Playing:
                CardManager.Instance.HideCardSelection();
                break;
            case GameSate.CardSelection:
                EventManager.Instance.DisableAllModifier();
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
