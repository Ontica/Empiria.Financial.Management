/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object                      Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialCostObjectTests                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialCostObject type.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.CostObject;
using Xunit;

//using Empiria.Financial.CostObjects;

namespace Empiria.Tests.Financial.CostObjects {

  /// <summary>Unit tests for FinancialCostObject type.</summary>
  public class FinancialCostObjectTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Financial_CostObject() {
      var list = BaseObject.GetFullList<FinancialCostObject>();
    }

    [Fact]
    public void Should_Read_All_Financial_CostObjects() {
      var sut = BaseObject.GetFullList<FinancialCostObject>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialCostObjects() {
      var sut = FinancialCostObject.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialCostObjectTests

}  // namespace Empiria.Tests.Financial.CostObjects
