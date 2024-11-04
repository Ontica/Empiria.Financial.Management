/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionEditionUseCases           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to edit budget transactions.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Services.Aspects;

using Empiria.Financial;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to edit budget transactions.</summary>
  public class BudgetTransactionEditionUseCases : WorkflowUseCase {

    #region Constructors and parsers

    protected BudgetTransactionEditionUseCases() {
      // no-op
    }

    static public BudgetTransactionEditionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetTransactionEditionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    [WorkflowEvent("BudgetTransactionCreated")]
    public void CreateTransaction(BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      base.SendWorkflowEvent("BudgetTransactionCreated", fields);
    }


    public BudgetTransaction CreateTransaction(IPayableEntity payable,
                                               BudgetTransactionFields fields) {

      Assertion.Require(fields, nameof(fields));
      Assertion.Require(payable, nameof(payable));

      var transactionBuilder = new BudgetTransactionBuilder(payable, fields);

      BudgetTransaction transaction = transactionBuilder.Build();

      transaction.Save();

      return transaction;
    }

    #endregion Use cases

  }  // class BudgetTransactionEditionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
