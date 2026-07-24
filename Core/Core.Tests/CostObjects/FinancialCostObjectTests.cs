/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object                      Component : Test cases                              *
*  Assembly : Empiria.Financial.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : FinancialCostObjectTests                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialCostObject type.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Financial.CostObject.Adapters;
using Empiria.Financial.CostObject.UseCases;
using Xunit;

namespace Empiria.Tests.Financial.CostObjects {

  /// <summary>Unit tests for FinancialCostObject type.</summary>
  public class FinancialCostObjectTests {

    #region Facts

    [Fact]
    public void Should_all_Financial_CostObject() {
      var sut = BaseObject.GetFullList<FinancialCostObject>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    [Fact]
    public void Should_Create_Financial_CostObjects() {

      TestsCommonMethods.Authenticate();

      var contact = ExecutionServer.CurrentContact;

      var entry = new FinancialCostObjectEntry {
        CostObjectTypeId = 5739,
        ExternalCode = "COM-022",
        Description = "Objeto de gasto de PRUEBA con COM-022",
        StartDate = DateTime.Today,
      };

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto sut = usecases.CreateCostObject(entry);

        Assert.NotNull(sut);
      }

    }

    [Fact]
    public void Should_Update_Financial_CostObjects() {
     
    }

    [Fact]
    public void Should_Delete_Financial_CostObjects() {

    }

    [Fact]
    public void Should_Read_Empty_FinancialCostObjects() {
      var sut = FinancialCostObject.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialCostObjectTests

}  // namespace Empiria.Tests.Financial.CostObjects
