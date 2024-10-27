/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Input Fields DTO                        *
*  Type     : BudgetTransactionFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update budget transactions.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Input fields DTO used to create and update budget transactions.</summary>
  public class BudgetTransactionFields : WorkItemDto {

    public string BaseBudgetUID {
      get; set;
    } = string.Empty;


    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public string TransactionSubTypeUID {
      get; set;
    } = string.Empty;


    public string BaseOrgUnitUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


  }  // BudgetTransactionFields

}  // namespace Empiria.Budgeting.Transactions.Adapters
