/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillUseCasesTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillUseCases.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Billing.Adapters;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Billing.UseCases;
using Xunit;

namespace Empiria.Tests.Billing {

  /// <summary></summary>
  public class BillUseCasesTests {


    [Fact]
    public void Should_Create_A_Bill_From_An_Xml_File() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillEntryDto sut = usecases.CreateBill(xmlFilePath);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_List_Test() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillsQuery query = GetBillsQuery();

        FixedList<BillEntryDto> sut = usecases.GetBillList(query);

        Assert.NotNull(sut);
      }
    }


    #region Helpers

    private BillsQuery GetBillsQuery() {

      return new BillsQuery {
        BillTypeUID = "",
        BillCategoryUID = "",
        ManagedByUID = "",
        Keywords ="",
        ConceptsKeywords="",
        Tags = new string[] { },
        BillDateType = BillQueryDateType.None,
        FromDate = new DateTime(2024,10,29),
        ToDate = new DateTime(2024, 10, 31),
        Status = Empiria.Billing.BillStatus.All
      };

    }

    #endregion Helpers


  } // class BillUseCasesTests

} // namespace Empiria.Tests.Billing
