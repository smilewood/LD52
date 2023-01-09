using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{

   public enum Upgrade
   {
      S_Speed,
      S_Vision,
      S_Intel,
      F_Move,
      F_Harvest,
      P_T1,
      P_T2
   }

   [Serializable]
   public struct UpgradeValues
   {
      [Header("Scarecrow")]
      public float MoveSpeed;
      public float FieldMod1;
      public float VisionRange;
      public int VisionPierce;
      public float FieldMod2;
      [Header("Player")]
      public float Speed;
      public float Harvest;
      [Header("Prices")]
      public int P1;
      public int P2;
   }

   
   public int wheatPrice;
   public int money;
   public UpdateTextOnMorning moneyText;
   public ScarecrowAI Scarecrow;
   public GameObject Player;
   public WheatfieldController field;
   public UpgradeValues Upgrades;


   private void Update()
   {
      if(Input.GetKey(KeyCode.M) && Input.GetKeyDown(KeyCode.N))
      {
         AddMoney(1);
      }
   }

   internal void AddMoney(int income)
   {
      money += income;
      moneyText.ManuallyUpdateText(money.ToString());
   }

   public void BuyUpgrade(Upgrade upgrade)
   {
      switch (upgrade)
      {
         case Upgrade.S_Speed:
         {
            Scarecrow.speed = Upgrades.MoveSpeed;
            field.SpawnOdds = Upgrades.FieldMod1;
            break;
         }
         case Upgrade.S_Vision:
         {
            Scarecrow.LOSRange = Upgrades.VisionRange;
            Scarecrow.LOSPierce = Upgrades.VisionPierce;
            field.SpawnOdds = Upgrades.FieldMod2;
            break;
         }
         case Upgrade.S_Intel:
         {
            MenuFunctions.Instance.ShowMenu("YouWin");
            break;
         }
         case Upgrade.F_Move:
         {
            Player.GetComponent<PlayerController>().Speed = Upgrades.Speed;
            break;
         }
         case Upgrade.F_Harvest:
         {
            Player.GetComponent<HarverstControlls>().HarvestTime = Upgrades.Harvest;
            break;
         }
         case Upgrade.P_T1:
         {
            wheatPrice = Upgrades.P1;
            break;
         }
         case Upgrade.P_T2:
         {
            wheatPrice = Upgrades.P2;
            break;
         }
      }

   }
}