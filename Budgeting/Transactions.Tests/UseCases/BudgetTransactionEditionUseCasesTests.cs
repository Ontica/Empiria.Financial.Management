/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Use cases tests                         *
*  Type     : BudgetTransactionEditionUseCasesTests      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use case tests for budget transactions edition.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.UseCases;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Use case tests for budget transactions edition.</summary>
  public class BudgetTransactionEditionUseCasesTests {

    #region Fields

    private readonly BudgetTransactionEditionUseCases _usecases;

    #endregion Fields

    #region Initialization

    public BudgetTransactionEditionUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = BudgetTransactionEditionUseCases.UseCaseInteractor();
    }

    ~BudgetTransactionEditionUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Initialization

    #region Facts

    //[Fact]
    //public void Should_Create_A_Budget_Transaction() {
    //  var fields = new BudgetTransactionFields {
    //    BaseBudgetUID = "bdd6ebe8-303c-41b3-917d-fdf49fb0e96e",
    //    WorkItemUID = "07288961-55d9-4e73-871e-7a48e8b0dac2"
    //  };

    //  _usecases.CreateTransaction(fields);
    //}

    #endregion Facts

  }  // class BudgetTransactionEditionUseCasesTests

}  // namespace Empiria.Tests.Budgeting.Transactions
