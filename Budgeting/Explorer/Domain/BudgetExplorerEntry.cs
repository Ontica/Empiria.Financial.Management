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

    public BudgetAccount BudgetAccount {
      get; internal set;
    }

    public OrganizationalUnit OrganizationalUnit {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public Currency Currency {
      get; internal set;
    }

    public decimal Planned {
      get; internal set;
    }

    public decimal Authorized {
      get; internal set;
    }

    public decimal Expanded {
      get; internal set;
    }

    public decimal Reduced {
      get; internal set;
    }

    public decimal Modified {
      get; internal set;
    }

    public decimal Requested {
      get; internal set;
    }

    public decimal Commited {
      get; internal set;
    }

    public decimal ToPay {
      get; internal set;
    }

    public decimal Excercised {
      get; internal set;
    }

    public decimal ToExercise {
      get; internal set;
    }

    public decimal Available {
      get; internal set;
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
