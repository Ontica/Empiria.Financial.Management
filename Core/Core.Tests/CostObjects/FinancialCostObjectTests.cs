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
      var sut = FinancialCostObject.GetList<FinancialCostObject>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    [Fact]
    public void Should_all_Financial_GetByAreaCostObject() {

      var sut = FinancialCostObject.GetList<FinancialCostObject>("COBJ_ORG_UNIT_ID = 495", "COBJ_DESCRIPTION");

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    [Fact]
    public void Should_all_Financial_GetTypes() {

      var sut = FinancialCostObjectType.GetList<FinancialCostObjectType>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    [Fact]
    public void Should_Create_Financial_CostObjects() {

      TestsCommonMethods.Authenticate();

      var contact = ExecutionServer.CurrentContact;

      var entry = new FinancialCostObjectEntry {
        CostObjectTypeId = 5739,
        ExternalCode = "COM-101",
        Description = "Objeto de gasto de PRUEBA con COM-101",
        requestedByUID = "22bf7ea2-4527-43ee-a5a6-6f3dd8d06646",
        StartDate = DateTime.Today,
      };

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto sut = usecases.CreateCostObject(entry);

        Assert.NotNull(sut);
      }

    }

    [Fact]
    public void Should_Update_Financial_CostObjects() {

      var entry = new FinancialCostObjectEntry {
        Description = "Objeto de gasto de PRUEBA con COM-100",
        requestedByUID = "fef1a792-94bc-44e6-8c6a-f5caa7c9a089",
        StartDate = DateTime.Today,
      };
      
      var UID = "e85a2713-2ae9-4705-bb8c-7c004bdeb1e1";

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto sut = usecases.UpdateCostObject(UID, entry);

        Assert.NotNull(sut);
      }

    }

    [Fact]
    public void Should_Delete_Financial_CostObjects() {
      
      var UID = "e85a2713-2ae9-4705-bb8c-7c004bdeb1e1";

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

       usecases.DeleteCostObject(UID);

      }
    }

    [Fact]
    public void Should_Read_Empty_FinancialCostObjects() {

      var UID = "e85a2713-2ae9-4705-bb8c-7c004bdeb1e1";
      var sut = FinancialCostObject.Parse(UID);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FinancialCostObjectTests

}  // namespace Empiria.Tests.Financial.CostObjects
