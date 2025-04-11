/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Query DTO                               *
*  Type     : BudgetTransactionAccountsQuery             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve budget transaction available accounts.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  public class BudgetTransactionAccountsQuery {

    public string TransactionUID {
      get; set;
    } = string.Empty;


    public string BudgetColumnUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class BudgetTransactionAccountsQuery

}  // namespace Empiria.Budgeting.Transactions.Adapters
