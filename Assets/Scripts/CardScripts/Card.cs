using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] SpriteRenderer cardImageRenderer;

    [SerializeField] TextMeshProUGUI cardTextRenderer;
    private CardSO cardInfo;
    public void Setup(CardSO card)
    {
        cardInfo = card;
        cardImageRenderer.sprite = card.cardImage; 
        cardTextRenderer.text = card.cardText;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("CardClicked");
        CardManager.Instance.SelectCard(cardInfo);
    }
}
