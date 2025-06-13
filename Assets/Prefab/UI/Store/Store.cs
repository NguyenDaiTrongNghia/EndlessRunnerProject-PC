using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI JumpBoostText;
    [SerializeField] TextMeshProUGUI SpeedBoostText;
    [SerializeField] TextMeshProUGUI MagnetText;
    //[SerializeField] TextMeshProUGUI NotEnoughText;
    //[SerializeField] TextMeshProUGUI MaximumText;
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] GameObject StorePanel;
    [SerializeField] TextMeshProUGUI StoreText;

    [SerializeField] int UpgradeCost = 100;
    [SerializeField] float UpgradeIncrement = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }
    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        float jumpDuration = SaveDataManager.GetPowerUpDuration("JumpBoostDuration");
        float speedDuration = SaveDataManager.GetPowerUpDuration("SpeedBoostDuration");
        float magnetDuration = SaveDataManager.GetPowerUpDuration("MagnetDuration");

        JumpBoostText.text = $"Jump: {jumpDuration:F0}s";
        SpeedBoostText.text = $"Speed: {speedDuration:F0}s";
        MagnetText.text = $"Magnet: {magnetDuration:F0}s";
    }
    public void BuyJumpBoostUpgrade()
    {
        UpgradePowerUp("JumpBoostDuration");
    }

    public void BuySpeedBoostUpgrade()
    {
        UpgradePowerUp("SpeedBoostDuration");
    }

    public void BuyMagnetUpgrade()
    {
        UpgradePowerUp("MagnetDuration");
    }

    private void UpgradePowerUp(string key)
    {
        int currentCoins = SaveDataManager.GetTotalCoins();
        float currentDuration = SaveDataManager.GetPowerUpDuration(key);

        if (currentDuration < 60f)
        {
            if (currentCoins >= UpgradeCost)
            {

                float newDuration = currentDuration + UpgradeIncrement;

                SaveDataManager.SavePowerUpDuration(key, newDuration);
                SaveDataManager.AddCoins(-UpgradeCost);
                mainMenuUI.UpdateTotalCoinDisplay();

                UpdateUI();
            }
            else
            {
                StorePanel.gameObject.SetActive(true);
                StoreText.text = "Not enough coins to upgrade";
                //Debug.Log("Not enough coins to upgrade " + key);
            }
        }
        else
        {
            StorePanel.gameObject.SetActive(true);
            StoreText.text = "Maximum upgrade reached";
            //Debug.Log("Maximum upgrade reached for " + key);
        }
    }

    public void CloseStorePanel()
    {
        StorePanel.gameObject.SetActive(false);
    }

    //public void CloseNotEnoughPanel()
    //{
    //    PanelNotEnough.gameObject.SetActive(false);
    //}
    //public void CloseMaximumPanel()
    //{
    //    PanelMaximum.gameObject.SetActive(false);
    //}
}
