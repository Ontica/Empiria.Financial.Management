/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Transactions.Tests.dll   Pattern   : Unit tests                              *
*  Type     : BudgetEntryTests                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BudgetEntry instances.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Tests.Budgeting.Transactions {

  /// <summary>Unit tests for BudgetEntry instances.</summary>
  public class BudgetEntryTests {

    #region Facts

    [Fact]
    public void Clean_Budget_Entries() {
      var budgetEntries = BaseObject.GetFullList<BudgetEntry>();

      foreach (var entry in budgetEntries) {
        BudgetTransactionDataService.CleanEntries(entry);
      }
    }


    [Fact]
    public void Should_Get_All_Budget_Entries() {
      var sut = BaseObject.GetFullList<BudgetEntry>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_BudgetEntry() {
      var sut = BudgetEntry.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class BudgetEntryTests

}  // namespace Empiria.Tests.Budgeting.Transactions
