using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    // Player's resources
    [SerializeField] private int nuclear;
    [SerializeField] private int gold;

    [SerializeField] private TextMeshProUGUI numberNuclear, numberGold;
    [SerializeField] private TextMeshProUGUI noMoreNuclearText;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        UpdateHUD();
    }

    public void UseNuclear()
    {
        nuclear--;
        UpdateHUD();
    }

    public void StoreNuclear()
    {
        nuclear++;
        UpdateHUD();
    }

    public void AddGold(int bonus)
    {
        gold += bonus;
        UpdateHUD();
    }

    public void UseGold(int cost)
    {
        gold -= cost;
        UpdateHUD();
    }
    
    public bool CheckNuclear()
    {
        if(nuclear  <= 0)
        {
            // No nuclear left

            StartCoroutine("ShowingNuclearText");

            return false;
        }
        else
        {
            // Some nuclear in stock
            return true;
        }
    }

    public int CheckGold()
    {
        return gold;
    }

    // UI MANAGER

    private void UpdateHUD()
    {
        numberGold.text = gold.ToString();
        numberNuclear.text = nuclear.ToString();
    }

    private IEnumerator ShowingNuclearText()
    {
        noMoreNuclearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        noMoreNuclearText.gameObject.SetActive(false);
    }

}
