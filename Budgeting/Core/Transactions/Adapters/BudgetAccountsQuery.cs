/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Input query DTO                         *
*  Type     : BudgetAccountsQuery                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve budget available accounts.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Input query DTO used to retrieve budget available accounts.</summary>
  public class BudgetAccountsQuery {

    public string TransactionUID {
      get; set;
    } = string.Empty;


    public string BaseBudgetUID {
      get; set;
    } = string.Empty;


    public string BasePartyUID {
      get; set;
    } = string.Empty;


    public BudgetOperationType OperationType {
      get; set;
    } = BudgetOperationType.Request;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public bool OnlyAssignedAccounts {
      get {
        return TransactionUID.Length == 0;
      }
    }


    internal void EnsureValid() {
      if (TransactionUID.Length == 0) {
        return;
      }

      var transaction = BudgetTransaction.Parse(TransactionUID);

      if (BaseBudgetUID.Length == 0) {
        BaseBudgetUID = transaction.BaseBudget.UID;
      }

      if (BasePartyUID.Length == 0) {
        BasePartyUID = transaction.BaseParty.UID;
      }
    }


    internal Budget GetBaseBudget() {
      return Budget.Parse(BaseBudgetUID);
    }


    internal OrganizationalUnit GetBaseParty() {
      return OrganizationalUnit.Parse(BasePartyUID);
    }


    internal BudgetTransactionType GetTransactionType() {
      if (TransactionUID.Length != 0) {
        var transaction = BudgetTransaction.Parse(TransactionUID);

        return transaction.BudgetTransactionType;
      }

      var budget = GetBaseBudget();

      return BudgetTransactionType.GetFor(budget.BudgetType, OperationType);
    }

  }  // class BudgetAccountsQuery

}  // namespace Empiria.Budgeting.Transactions.Adapters
