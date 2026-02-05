/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Information Holder                      *
*  Type     : BudgetExplorerEntry                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a dynamic explorer entry.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Budgeting.Explorer {

  public class BudgetExplorerEntry {

    internal BudgetExplorerEntry(BudgetExplorerEntry sourceData) {
      Sum(sourceData);

      Year = sourceData.Year;
      Month = sourceData.Month;

      ClickableEntry = true;
      ItemType = "Entry";
      Title = sourceData.OrganizationalUnit.FullName;
      Description = sourceData.BudgetAccount.Name;
      UID = $"{sourceData.OrganizationalUnit.Id}|{sourceData.BudgetAccount.Id}";
    }


    internal BudgetExplorerEntry(BudgetDataInColumns sourceData, bool fullLoad) {

      if (fullLoad) {
        BudgetAccount = sourceData.BudgetAccount;
        OrganizationalUnit = sourceData.BudgetAccount.OrganizationalUnit;
        Year = sourceData.Year;
        Month = sourceData.Month;
        UID = $"{OrganizationalUnit.Id}|{BudgetAccount.Id}";
        Title = OrganizationalUnit.FullName;
        Description = BudgetAccount.Name;
      }

      Planned = sourceData.Planned;
      Authorized = sourceData.Authorized;
      Expanded = sourceData.Expanded;
      Reduced = sourceData.Reduced;
      Modified = sourceData.Modified;
      Requested = sourceData.Requested;
      Commited = sourceData.Commited;
      ToPay = sourceData.ToPay;
      Excercised = sourceData.Excercised;
      ToExercise = sourceData.ToExercise;
      Available = sourceData.Available;

      ClickableEntry = true;
      ItemType = "Entry";
    }


    public string UID {
      get;
    }

    public bool ClickableEntry {
      get;
    }

    public string ItemType {
      get;
    }

    public string Title {
      get;
    }

    public string Description {
      get;
    }

    public BudgetAccount BudgetAccount {
      get; private set;
    }

    public OrganizationalUnit OrganizationalUnit {
      get; private set;
    }

    public int Year {
      get; private set;
    }

    public int Month {
      get; private set;
    }


    public string MonthName {
      get {
        if (Month == 0) {
          return Year.ToString();
        }
        return EmpiriaString.MonthName(Month);
      }
    }


    public decimal Planned {
      get; private set;
    }

    public decimal Authorized {
      get; private set;
    }

    public decimal Expanded {
      get; private set;
    }

    public decimal Reduced {
      get; private set;
    }

    public decimal Modified {
      get; private set;
    }

    public decimal Requested {
      get; private set;
    }

    public decimal Commited {
      get; private set;
    }

    public decimal ToPay {
      get; private set;
    }

    public decimal Excercised {
      get; private set;
    }

    public decimal ToExercise {
      get; private set;
    }

    public decimal Available {
      get; private set;
    }


    internal void Sum(BudgetExplorerEntry entry) {
      Planned += entry.Planned;
      Authorized += entry.Authorized;
      Expanded += entry.Expanded;
      Reduced += entry.Reduced;
      Modified += entry.Modified;
      Requested += entry.Requested;
      Commited += entry.Commited;
      ToPay += entry.ToPay;
      Excercised += entry.Excercised;
      ToExercise += entry.ToExercise;
      Available += entry.Available;
    }

  }  // class BudgetExplorerEntry

}  // namespace Empiria.Budgeting.Explorer
