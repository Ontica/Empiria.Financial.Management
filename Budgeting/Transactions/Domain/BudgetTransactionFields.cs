/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Input Fields DTO                        *
*  Type     : BudgetTransactionFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update budget transactions.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Input fields DTO used to create and update budget transactions.</summary>
  public class BudgetTransactionFields : WorkItemDto {

    public string TransactionTypeUID {
      get; set;
    } = string.Empty;


    public string BaseBudgetUID {
      get; set;
    } = string.Empty;


    public string BasePartyUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string OperationSourceUID {
      get; set;
    } = string.Empty;


    public int ContractId {
      get; set;
    } = -1;


    public int PayableId {
      get; set;
    } = -1;


    public DateTime ApplicationDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string BaseEntityTypeUID {
      get; set;
    } = string.Empty;


    public string BaseEntityUID {
      get; set;
    } = string.Empty;

  }  // BudgetTransactionFields



  /// <summary>Extension methods for BudgetTransactionFields class.</summary>
  static internal class BudgetTransactionFieldsExtension {

    static internal void EnsureIsValid(this BudgetTransactionFields fields) {
      fields.Description = EmpiriaString.Clean(fields.Description);
      fields.Justification = EmpiriaString.Clean(fields.Justification);

      if (fields.TransactionTypeUID.Length != 0) {
        _ = BudgetTransactionType.Parse(fields.TransactionTypeUID);
      }

      if (fields.BaseBudgetUID.Length != 0) {
        _ = Budget.Parse(fields.BaseBudgetUID);
      }

      if (fields.OperationSourceUID.Length != 0) {
        _ = OperationSource.Parse(fields.OperationSourceUID);
      }

      if (fields.BasePartyUID.Length != 0) {
        _ = Party.Parse(fields.BasePartyUID);
      }

      if (fields.RequestedByUID.Length != 0) {
        _ = Party.Parse(fields.RequestedByUID);
      }
    }

  }  // class BudgetTransactionFieldsExtensions

}  // namespace Empiria.Budgeting.Transactions
