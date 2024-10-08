/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionExecutionUseCases         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to record budget transactions.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Services.Aspects;

using Empiria.Budgeting.Transactions.Execution.Adapters;

namespace Empiria.Budgeting.Transactions.Execution.UseCases {

  /// <summary>Use cases used to record budget transactions.</summary>
  public class BudgetTransactionExecutionUseCases : WorkflowUseCase {

    #region Constructors and parsers

    protected BudgetTransactionExecutionUseCases() {
      // no-op
    }

    static public BudgetTransactionExecutionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetTransactionExecutionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    [WorkflowEvent("BudgetTransactionCreated")]
    public void CreateTransaction(BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      base.SendWorkflowEvent("BudgetTransactionCreated", fields);
    }

    #endregion Use cases

  }  // class BudgetTransactionExecutionUseCases

}  // namespace Empiria.Budgeting.Transactions.Execution.UseCases
