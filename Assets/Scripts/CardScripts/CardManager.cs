using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardSelectionUI;

    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform cardPositionOne;
    [SerializeField] Transform cardPositionTwo;
    [SerializeField] Transform cardPositionThree;

    [SerializeField] List<CardSO> deck;

    GameObject cardOne, cardTwo, cardThree;

    List<CardSO> alreadySelectedCards = new List<CardSO>();

    public static CardManager Instance;
    public UnityEvent StartNextWave;

    private void Awake()
    {
        Instance = this;

        if(GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += HandleGameStateChanged;
        
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameSate state)
    {
        if(state == GameManager.GameSate.CardSelection)
        {
            RandomizeNewCards();
        }
    }

    void RandomizeNewCards()
    {
        if(cardOne != null) Destroy(cardOne);
        if(cardTwo != null) Destroy(cardTwo);
        if(cardThree != null) Destroy(cardThree);

        List<CardSO> randomizedCards = new List<CardSO>();

        List<CardSO> availableCards = new List<CardSO>(deck);
        availableCards.RemoveAll(card =>
            card.isUnique && alreadySelectedCards.Contains(card) 
            || card.unlockLevel > GameManager.Instance.GetCurrentLevel()
        );

        if(availableCards.Count < 3)
        {
            Debug.Log("Pas assez de carte disponible");
            return;
        }

        while(randomizedCards.Count < 3)
        {
            CardSO randomCard = availableCards[Random.Range(0, availableCards.Count)];
            if(!randomizedCards.Contains(randomCard))
            {
                randomizedCards.Add(randomCard);
            }

        }

        cardOne = InstantiateCard(randomizedCards[0], cardPositionOne);
        cardTwo = InstantiateCard(randomizedCards[1], cardPositionTwo);
        cardThree = InstantiateCard(randomizedCards[2], cardPositionThree);
    }

    GameObject InstantiateCard(CardSO cardSO, Transform position) //créer les 3 cartes au position définie
    {
        GameObject cardGo = Instantiate(cardPrefab, position.position, Quaternion.identity, position);
        Card card = cardGo.GetComponent<Card>();
        card.Setup(cardSO);
        return cardGo;
    }

    public void SelectCard(CardSO selectedCard) //Fonction de la selection des cartes qui renvoie la carte slectionné avec son effet
    {
        if(!alreadySelectedCards.Contains(selectedCard))
        {
            //alreadySelectedCards.Add(selectedCard);

            ApplyEffect(selectedCard);
        }

        GameManager.Instance.ChangeState(GameManager.GameSate.Playing);
        StartNextWave.Invoke();
    }

    public void ShowCardSelection() //Montrer le canvas des cartes
    {
        cardSelectionUI.SetActive(true);
    }

    public void HideCardSelection()//cacher le canvas des cartes
    {
        cardSelectionUI.SetActive(false);
    }

    public void ApplyEffect(CardSO selectedCard)//appliquer l'effet sur le EventManager
    {
        EventManager.Instance.ShowEffect(selectedCard);
    }
}
