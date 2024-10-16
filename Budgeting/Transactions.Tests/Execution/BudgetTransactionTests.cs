/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions Tests                  Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Use cases tests                         *
*  Type     : BudgetTransactionTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetTransaction instances.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Unit tests for BudgetTransaction instances.</summary>
  public class BudgetTransactionTests {

    #region Facts

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
