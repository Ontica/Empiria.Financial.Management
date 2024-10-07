/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions Tests                  Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Use cases tests                         *
*  Type     : BudgetTransactionExecutionUseCasesTests    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Budget transactions execution use cases tests.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Budgeting.Transactions.Execution.Adapters;
using Empiria.Budgeting.Transactions.Execution.UseCases;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Budget transactions execution use cases tests.</summary>
  public class BudgetTransactionExecutionUseCasesTests {

    #region Fields

    private readonly BudgetTransactionExecutionUseCases _usecases;

    #endregion Fields

    #region Initialization

    public BudgetTransactionExecutionUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = BudgetTransactionExecutionUseCases.UseCaseInteractor();
    }

    ~BudgetTransactionExecutionUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Initialization

    #region Facts

    [Fact]
    public void Should_Create_A_Budget_Transaction() {
      var fields = new BudgetTransactionFields {
        WorkItemUID = "12309481209840918408-123"
      };

      _usecases.CreateTransaction(fields);
    }

    #endregion Facts

  }  // class BudgetTransactionExecutionUseCasesTests

}  // namespace Empiria.Tests.Budgeting.Transactions
