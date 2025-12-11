using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    // Player's resources
    [SerializeField] private int nuclear;
    [SerializeField] private int gold;

    [SerializeField] private List<GameObject> NuclearImages = new List<GameObject>();
    [SerializeField] private List<GameObject> VisibleImages = new List<GameObject>();

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

    public void RegisterImage(GameObject image)
    {
        if (image == null) return;
        if (!NuclearImages.Contains(image))
        {
            NuclearImages.Add(image);
            UpdateNuclearImages();
        }
    }

    public void UnregisterImage(int index)
    {
        if (index < 0 && index >= nuclear)
        {
            nuclear--;
            NuclearImages[index].SetActive(false);
        }
    }

    private void UpdateNuclearImages()
    {
        for (int i = 0; i < NuclearImages.Count; i++)
        {
            bool shouldBeActive = i < nuclear;
            if (NuclearImages[i] != null)
                NuclearImages[i].SetActive(shouldBeActive);
        }
    }

    private void Start()
    {
        UpdateHUD();
    }

    public void UseNuclear()
    {
        nuclear = Mathf.Max(0, nuclear - 1);
        UnregisterImage(nuclear);
        UpdateNuclearImages();
        UpdateHUD();
    }

    public void StoreNuclear()
    {
        nuclear = Mathf.Max(0, nuclear + 1);
        UpdateNuclearImages();
        UpdateHUD(); ;
    }

    public void AddNuclear(int bonus)
    {
        nuclear = Mathf.Clamp(nuclear + bonus, 0, NuclearImages.Count);
        UpdateNuclearImages();
        UpdateHUD();
    }

    public void RemoveNuclear(int cost)
    {
        nuclear = Mathf.Max(0, nuclear - cost);
        UpdateNuclearImages();
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
        if (noMoreNuclearText != null)  
        noMoreNuclearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (noMoreNuclearText != null)
            noMoreNuclearText.gameObject.SetActive(false);
    }

}
