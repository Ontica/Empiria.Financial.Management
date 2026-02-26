/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillUseCasesTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillUseCases.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;
using Empiria.Billing;

namespace Empiria.Tests.Billing {

  /// <summary></summary>
  public class BillUseCasesTests {


    //[Fact]
    //public void Create_Bill_Test() {

    //  TestsCommonMethods.Authenticate();

    //  using (var usecases = BillUseCases.UseCaseInteractor()) {

    //    string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

    //    var xmlText = System.IO.File.ReadAllText(xmlFilePath);

    //    BillDto sut = usecases.CreateBillTest(xmlText);

    //    Assert.NotNull(sut);
    //  }
    //}


    //[Fact]
    //public void Create_Credit_Note_Test() {

    //  TestsCommonMethods.Authenticate();

    //  using (var usecases = BillUseCases.UseCaseInteractor()) {

    //    string xmlFilePath = TestingConstants.XML_CREDIT_NOTE_FILE_PATH;

    //    var xmlText = System.IO.File.ReadAllText(xmlFilePath);

    //    BillDto sut = usecases.CreateBillTest(xmlText);

    //    Assert.NotNull(sut);
    //  }
    //}


    //[Fact]
    //public void Create_Fuel_Consumption_Bill_Test() {

    //  TestsCommonMethods.Authenticate();

    //  using (var usecases = BillUseCases.UseCaseInteractor()) {

    //    string xmlFilePath = TestingConstants.XML_FUEL_CONSUMPTION_BILL_FILE_PATH;

    //    var xmlText = System.IO.File.ReadAllText(xmlFilePath);

    //    BillDto sut = usecases.CreateFuelConsumptionBillTest(xmlText);

    //    Assert.NotNull(sut);
    //  }
    //}


    //[Fact]
    //public void Create_Payment_Complement_Test() {

    //  TestsCommonMethods.Authenticate();

    //  using (var usecases = BillUseCases.UseCaseInteractor()) {

    //    string xmlFilePath = TestingConstants.XML_PAYMENT_COMPLEMENT_FILE_PATH;

    //    var xmlText = System.IO.File.ReadAllText(xmlFilePath);

    //    BillDto sut = usecases.CreateBillPaymentComplementTest(xmlText);

    //    Assert.NotNull(sut);
    //  }
    //}


    [Fact]
    public void Get_Bill_By_UID_Test() {

      TestsCommonMethods.Authenticate();

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        string[] billsUID = new string[] {
          "3bc2aff6-b179-439b-a750-aa7a3190bd83"
        };

        BillsStructureDto sut = usecases.GetBills(billsUID);

        Assert.NotNull(sut);
      }
    }


    [Fact]
    public void Delete_Bill_Test() {

      TestsCommonMethods.Authenticate();

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        usecases.DeleteBill("12972994-0808-4240-b4b3-dbf33c403d4f");

        Assert.True(true);
      }
    }

    #region Helpers

    #endregion Helpers


  } // class BillUseCasesTests

} // namespace Empiria.Tests.Billing
