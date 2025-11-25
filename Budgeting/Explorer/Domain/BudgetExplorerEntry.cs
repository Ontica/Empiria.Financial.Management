/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Information Holder                      *
*  Type     : BudgetExplorerEntry                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a dynamic explorer entry.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.Budgeting.Explorer {

  public class BudgetExplorerEntry {

    internal BudgetExplorerEntry(BudgetExplorerEntry sourceData) {
      Sum(sourceData);
      ////Currency = sourceData.Currency;
      ////OrganizationalUnit = OrganizationalUnit.Parse(sourceData.BudgetAccount.Segment_1.Id);
      Year = sourceData.Year;
      Month = sourceData.Month;
    }


    internal BudgetExplorerEntry(BudgetDataInColumns sourceData, bool fullLoad) {
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

      if (fullLoad) {
        BudgetAccount = sourceData.BudgetAccount;
        Currency = sourceData.Currency;
        OrganizationalUnit = OrganizationalUnit.Parse(sourceData.BudgetAccount.OrganizationalUnit.Id);
        Year = sourceData.Year;
        Month = sourceData.Month;
      }
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

    public Currency Currency {
      get; private set;
    } = Currency.Empty;


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
