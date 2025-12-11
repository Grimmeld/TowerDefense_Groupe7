using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] SpriteRenderer cardImageRenderer;

    [SerializeField] TextMeshProUGUI cardTitle;
    [SerializeField] TextMeshProUGUI cardTextRenderer;
    [SerializeField] TextMeshProUGUI cardTextCostRenderer;
    private CardSO cardInfo;
    public void Setup(CardSO card)
    {
        cardInfo = card;
        cardTitle.text = card.cardName;
        cardImageRenderer.sprite = card.cardImage; 
        cardTextRenderer.text = card.cardText;
        cardTextCostRenderer.text = card.cardGoldText;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("CardClicked");
        CardManager.Instance.SelectCard(cardInfo);
    }
}
