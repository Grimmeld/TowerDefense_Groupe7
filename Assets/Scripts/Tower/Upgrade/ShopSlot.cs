using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
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


}
