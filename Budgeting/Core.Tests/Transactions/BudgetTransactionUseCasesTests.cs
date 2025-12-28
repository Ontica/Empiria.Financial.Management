/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Use cases tests                         *
*  Type     : BudgetTransactionUseCasesTests             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Budget transactions edition use cases tests.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Budget transactions edition use cases tests.</summary>
  public class BudgetTransactionUseCasesTests {

    #region Fields

    private readonly BudgetTransactionUseCases _usecases;

    #endregion Fields

    #region Initialization

    public BudgetTransactionUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = BudgetTransactionUseCases.UseCaseInteractor();
    }

    ~BudgetTransactionUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Initialization

    #region Facts

    [Fact]
    public void Should_Search_Budget_Transactions() {
      var query = new BudgetTransactionsQuery {
        Keywords = ""
      };

      FixedList<BudgetTransactionDescriptorDto> sut = _usecases.SearchTransactions(query);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetTransactionUseCasesTests

}  // namespace Empiria.Tests.Budgeting.Transactions
