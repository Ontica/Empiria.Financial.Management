/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Unit tests                              *
*  Type     : BudgetTransactionTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetTransaction instances.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Unit tests for BudgetTransaction instances.</summary>
  public class BudgetTransactionTests {

    #region Facts

    [Fact]
    public void Clean_Budget_Transactions() {
      var transactions = BaseObject.GetFullList<BudgetTransaction>();

      foreach (var transaction in transactions) {
        BudgetTransactionDataService.CleanTransaction(transaction);
      }
    }


    [Fact]
    public void Should_Get_All_Budget_Transactions() {
      var sut = BaseObject.GetFullList<BudgetTransaction>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_BudgetTransaction() {
      var sut = BudgetTransaction.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class BudgetTransactionTests

}  // namespace Empiria.Tests.Budgeting.Transactions
