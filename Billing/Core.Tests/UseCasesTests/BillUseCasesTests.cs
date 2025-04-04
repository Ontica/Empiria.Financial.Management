﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillUseCasesTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillUseCases.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

namespace Empiria.Tests.Billing {

  /// <summary></summary>
  public class BillUseCasesTests {


    [Fact]
    public void Create_Bill_Test() {

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

        var xmlText = System.IO.File.ReadAllText(xmlFilePath);

        BillDto sut = usecases.CreateBillTest(xmlText);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Create_Payment_Complement_Test() {

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        string xmlFilePath = TestingConstants.XML_PAYMENT_COMPLEMENT_FILE_PATH;

        var xmlText = System.IO.File.ReadAllText(xmlFilePath);

        BillDto sut = usecases.CreateBillPaymentComplementTest(xmlText);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_By_UID_Test() {

      string billUID = "a8161e85-b760-42a4-a170-1a867e97392a";

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillHolderDto sut = usecases.GetBill(billUID);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_Map_By_UID_Test() {

      string billUID = "45010561-add4-46ca-a687-1ac4ed5885ca";

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillWithConceptsDto sut = usecases.GetBillWithConceptsDto(billUID);
        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Get_Bill_List_Test() {

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillsQuery query = GetBillsQuery();

        FixedList<BillDescriptorDto> sut = usecases.SearchBills(query);

        Assert.NotNull(sut);
      }
    }


    #region Helpers

    private BillsQuery GetBillsQuery() {

      return new BillsQuery {
        BillTypeUID = "",
        BillCategoryUID = "",
        ManagedByUID = "",
        Keywords = "FACTURA ONTICA",
        ConceptsKeywords = "",
        Tags = new string[] { },
        BillDateType = BillQueryDateType.None,
        //FromDate = new DateTime(2024, 10, 29),
        //ToDate = new DateTime(2024, 10, 31),
        Status = Empiria.Billing.BillStatus.Pending
      };

    }

    #endregion Helpers


  } // class BillUseCasesTests

} // namespace Empiria.Tests.Billing
