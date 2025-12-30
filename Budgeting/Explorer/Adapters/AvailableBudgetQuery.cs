/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Input query DTO                         *
*  Type     : AvailableBudgetQuery                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query for request available budget information.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer.Adapters {

  /// <summary>Input query for request available budget information.</summary>
  public class AvailableBudgetQuery {

    public Budget Budget {
      get; set;
    } = Budget.Empty;


    public int Year {
      get; set;
    }

    public int Month {
      get; set;
    }

    public FixedList<BudgetAccount> Accounts {
      get; set;
    } = FixedList<BudgetAccount>.Empty;

  }  // class AvailableBudgetQuery

}  // namespace Empiria.Budgeting.Explorer.Adapters
