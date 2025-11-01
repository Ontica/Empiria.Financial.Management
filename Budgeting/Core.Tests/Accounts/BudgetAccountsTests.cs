/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : BudgetAccountsTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for budget accounts.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting;
using Empiria.Budgeting.Data;

namespace Empiria.Tests.Budgeting {

  /// <summary>Unit tests for budget accounts.</summary>
  public class BudgetAccountsTests {

    #region Facts

    [Fact]
    public void Clean_Budget_Accounts() {
      var accounts = BaseObject.GetFullList<FormerBudgetAccount>();

      foreach(var account in accounts) {
        FormerBudgetAcctData.CleanAccount(account);
      }
    }


    [Fact]
    public void Should_Read_Empty_BudgetAccount() {
      var sut = FormerBudgetAccount.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_All_Budget_Accounts() {
      var sut = FormerBudgetAccount.GetList<FormerBudgetAccount>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetAccountsTests

}  // namespace Empiria.Tests.Budgeting
