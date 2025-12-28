/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Unit tests                              *
*  Type     : BudgetTransactionTypeTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetTransactionType instances.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Unit tests for BudgetTransactionType instances.</summary>
  public class BudgetTransactionTypeTests {

    #region Facts

    [Fact]
    public void Should_Get_All_Budget_Transaction_Types() {
      var sut = BudgetTransactionType.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_BudgetTransactionType() {
      var sut = BudgetTransactionType.Empty;

      Assert.NotNull(sut);
      Assert.NotNull(sut.BudgetType);
    }


    [Fact]
    public void Should_Parse_All_Budget_Transaction_Types() {
      var transactionTypes = BudgetTransactionType.GetList();

      foreach (var sut in transactionTypes) {
        Assert.NotNull(sut.BudgetType);
      }
    }

    #endregion Facts

  }  // class BudgetTransactionTypeTests

}  // namespace Empiria.Tests.Budgeting.Transactions
