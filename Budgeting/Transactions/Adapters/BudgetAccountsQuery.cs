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


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string OperationType {
      get; set;
    } = "procurement";


    public string Keywords {
      get; set;
    } = string.Empty;


    public bool OnlyAssignedAccounts {
      get {
        return TransactionUID.Length == 0;
      }
    }

    internal void EnsureValid() {
      if (TransactionUID.Length != 0) {
        var transaction = BudgetTransaction.Parse(TransactionUID);
        BaseBudgetUID = transaction.BaseBudget.UID;
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

      var transactionType = budget.BudgetType.GetTransactionTypes()
                                             .Select(x => BudgetTransactionType.Parse(x.UID))
                                             .ToFixedList()
                                             .Find(x => x.OperationTypes.ToLower().Contains(OperationType.ToLower()));

      if (transactionType == null) {
        Assertion.EnsureNoReachThisCode(
          $"The operation type '{OperationType}' is not valid for budget type '{budget.BudgetType.Name}'.");
      }

      return transactionType;
    }

  }  // class BudgetAccountsQuery

}  // namespace Empiria.Budgeting.Transactions.Adapters
