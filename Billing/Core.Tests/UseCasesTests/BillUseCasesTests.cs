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
    public void Create_A_Bill_From_An_Xml_File_Test() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillDto sut = usecases.CreateBill(xmlFilePath);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_By_UID_Test() {

      string billUID = "52c7394e-9d1b-4632-89df-320d2e438874";

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillHolderDto sut = usecases.GetBill(billUID);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_Map_By_UID_Test() {

      string billUID = "52c7394e-9d1b-4632-89df-320d2e438874";

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillWithConceptsDto sut = usecases.GetBillWithConceptsDto(billUID);
        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_List_Test() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillsQuery query = GetBillsQuery();

        FixedList<BillDescriptorDto> sut = usecases.GetBillList(query);

        Assert.NotNull(sut);
      }
    }


    #region Helpers

    private BillsQuery GetBillsQuery() {

      return new BillsQuery {
        BillTypeUID = "",
        BillCategoryUID = "",
        ManagedByUID = "",
        Keywords = "",
        ConceptsKeywords = "",
        Tags = new string[] { },
        BillDateType = BillQueryDateType.None,
        FromDate = new DateTime(2024, 10, 29),
        ToDate = new DateTime(2024, 10, 31),
        Status = Empiria.Billing.BillStatus.All
      };

    }

    #endregion Helpers


  } // class BillUseCasesTests

} // namespace Empiria.Tests.Billing
