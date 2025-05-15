/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Service provider                        *
*  Type     : BudgetTransactionsGenerator                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to generate budget transactions automatically.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Parties;

using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services to generate budget transactions automatically.</summary>
  internal class BudgetTransactionsGenerator {

    internal BudgetTransactionsGenerator() {

    }

    internal FixedList<BudgetTransaction> GenerateForPlanning(Budget budget) {
      FixedList<INamedEntity> planningTxnTypes = budget.PlanningAutoGenerationTransactionTypes;

      var generated = new List<BudgetTransaction>(255);

      foreach (var planningTxnType in planningTxnTypes) {
        BudgetTransactionType transactionType = BudgetTransactionType.Parse(planningTxnType.UID);

        generated.AddRange(GenerateForPlanning(budget, transactionType));

      }

      return generated.ToFixedList();
    }


    #region Helpers

    private List<BudgetTransaction> GenerateForPlanning(Budget budget, BudgetTransactionType transactionType) {
      var orgUnits = MissedTransactionOrgUnits(budget, transactionType);

      var generated = new List<BudgetTransaction>(orgUnits.Count);

      foreach (var orgUnit in orgUnits) {
        var transaction = new BudgetTransaction(transactionType, budget);
        var fields = new BudgetTransactionFields {
          BasePartyUID = orgUnit.UID,
          OperationSourceUID = OperationSource.Parse(11).UID,
        };

        transaction.Update(fields);

        generated.Add(transaction);
      }

      transactionType = BudgetTransactionType.Parse(3565);

      return generated;
    }


    private FixedList<OrganizationalUnit> GetBudgetingOrgUnits() {
      return BaseObject.GetFullList<OrganizationalUnit>()
                       .FindAll(x => x.ExtendedData.HasValue("budgetProgram") &&
                                     x.ExtendedData.Get("budgetProgram", "N/A") != "N/A");
    }


    private FixedList<OrganizationalUnit> MissedTransactionOrgUnits(Budget budget,
                                                                    BudgetTransactionType transactionType) {
      FixedList<OrganizationalUnit> orgUnits = GetBudgetingOrgUnits();

      FixedList<BudgetTransaction> currentTransactions = BudgetTransactionDataService.GetTransactions(budget, transactionType);

      return orgUnits.FindAll(x => !currentTransactions.Contains(y => y.BaseParty.Equals(x)));
    }

    #endregion Helpers

  }  // class BudgetTransactionsGenerator

}  // namespace Empiria.Budgeting.Transactions
