using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Module module;
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI slotGold;

   private void OnEnable()
    {
        if (module != null)
        {
            if (slotImage != null)
            {
                slotImage.sprite = module.sprite;
            }

            slotGold.text = module.price.ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Slot hover enter");
        if (UpgradeMenu.Instance != null)
        {
            UpgradeMenu.Instance.HoverSlot(true);
            if (StatModule.Instance != null) { StatModule.Instance.SetInformation(module); }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Slot hover exit");
        if (UpgradeMenu.Instance != null)
        {
            UpgradeMenu.Instance.HoverSlot(false);

        }
    }
}
