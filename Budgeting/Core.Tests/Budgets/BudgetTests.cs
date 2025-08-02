/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : BudgetTests                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Budget type.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting;

namespace Empiria.Tests.Budgeting {

  /// <summary>Unit tests for Budget type.</summary>
  public class BudgetTests {

    #region Facts

    [Fact]
    public void Should_Read_Empty_Budget() {
      var sut = Budget.Empty;

      Assert.NotNull(sut);
      Assert.Equal(-1, sut.Id);
    }


    [Fact]
    public void Should_Read_All_Budgets() {
      var sut = BaseObject.GetList<Budget>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class BudgetTests

}  // namespace Empiria.Tests.Budgeting
