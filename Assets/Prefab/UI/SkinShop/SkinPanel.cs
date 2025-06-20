using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinPanel : MonoBehaviour
{
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] TextMeshProUGUI EquipSkinBtn;
    [SerializeField] int skinIndex;
    [SerializeField] int SkinCost = 1000;
    private bool isSkinUnlocked;

    [SerializeField] GameObject PanelNotEnough;

    void Start()
    {
        isSkinUnlocked = SaveDataManager.IsSkinUnlocked(skinIndex);
        UpdateButtonUI();
        PanelNotEnough.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("SelectedSkin", 0) == skinIndex)
        {
            EquipSkinBtn.text = "Equipped";
        }
        else if (isSkinUnlocked)
        {
            EquipSkinBtn.text = "Equip";
        }
    }

    public void OnSkinButtonPress()
    {
        int currentCoins = SaveDataManager.GetTotalCoins();

        if (isSkinUnlocked)
        {
            EquipSkin();
            EquipSkinBtn.text = "Equipped";
        }
        else
        {
            if (currentCoins >= SkinCost)
            {
                SaveDataManager.AddCoins(-SkinCost);
                mainMenuUI.UpdateTotalCoinDisplay();
                SaveDataManager.UnlockSkin(skinIndex);
                //Debug.Log("Unlock skin: " + skinIndex);
                isSkinUnlocked = true;

                UpdateButtonUI();
            } 
            else
            {
                PanelNotEnough.gameObject.SetActive(true);
                //Debug.Log("Not enough coins to buy " + skinIndex);
            }          
        }
    }
    public void EquipSkin()
    {
        SaveDataManager.EquipSkinData(skinIndex);
        //Debug.Log("Equipped skin: " + skinIndex);
    }

    private void UpdateButtonUI()
    {
        EquipSkinBtn.text = isSkinUnlocked ? "Equip" : SkinCost + " Coins";
    }
    public void CloseNotEnoughPanel()
    {
        PanelNotEnough.gameObject.SetActive(false);
    }
}
