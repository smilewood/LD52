using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeToggle : UpdateOnMorning
{
   static List<UpgradeToggle> allToggles;

   public UpgradeMenu.Upgrade Upgrade;
   public TMP_Text priceText;
   public int price;
   public GameObject LockIcon;
   public bool locked;
   private bool purchaced;
   private Toggle toggle;
   private UpgradeMenu upgrades;
   private bool initialized = false;
   private void Initialize()
   {
      if (initialized)
      {
         return;
      }
      if(allToggles is null)
      {
         allToggles = new List<UpgradeToggle>();
      }
      allToggles.Add(this);
      toggle = GetComponentInChildren<Toggle>();
      LockIcon.SetActive(locked);
      upgrades = GetComponentInParent<UpgradeMenu>();
      priceText.text = price.ToString();
      initialized = true;
   }

   private void Awake()
   {
      Initialize();
   }

   public void SetLock(bool shouldLock)
   {
      locked = shouldLock;
      LockIcon.SetActive(locked);
      UpdateInfo(upgrades);
   }

   public override void UpdateInfo(MorningReport Report)
   {
      UpdateInfo(Report.upgrades);
   }

   private void UpdateInfo(UpgradeMenu info)
   {
      Initialize();
      toggle.interactable = !locked && !purchaced && info.money >= price;
      toggle.isOn = purchaced;
   }

   public void OnToggled(bool toggleState)
   {
      upgrades.AddMoney(-price);
      upgrades.BuyUpgrade(Upgrade);
      this.purchaced = true;
      allToggles.ForEach(t => t.UpdateInfo(upgrades));
   }
}
