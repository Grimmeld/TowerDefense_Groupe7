using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SpriteRenderer cardImageRenderer;

    [SerializeField] TextMeshProUGUI cardTitle;
    [SerializeField] TextMeshProUGUI cardTextRenderer;
    [SerializeField] TextMeshProUGUI cardTextCostRenderer;
    [SerializeField] private Animator animator;
    private CardSO cardInfo;

    void Awake()
    {
               animator = GetComponent<Animator>();
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("In", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("In", false);
    }
}
