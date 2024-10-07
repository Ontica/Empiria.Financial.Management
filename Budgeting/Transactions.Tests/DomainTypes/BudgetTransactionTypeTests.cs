/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Unit tests                              *
*  Type     : BudgetTransactionTypeTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for budget transaction types.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Unit tests for budget transaction types.</summary>
  public class BudgetTransactionTypeTests {

    #region Facts

    [Fact]
    public void Should_Read_Empty_Budget_TransactionType() {
      var sut = BudgetTransactionType.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_All_Budget_Transaction_Types() {
      var sut = BaseObject.GetList<BudgetTransactionType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetTransactionTypeTests

}  // namespace Empiria.Tests.Budgeting.Transactions
