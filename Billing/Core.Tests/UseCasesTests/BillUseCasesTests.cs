/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillUseCasesTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillUseCases.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Billing;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;
using Empiria.Documents;
using Xunit;

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


    [Fact]
    public void Get_Bill_By_UID_Test() {

      TestsCommonMethods.Authenticate();

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        string[] billsUID = new string[] {
          "f7efd60c-a6e2-4e94-a95e-d940cee8018c",
          "dbc189cf-bed0-4474-bba7-ce6fd43136cb",
          "3de8b8c1-c3d5-4963-89b1-6e5168964c39"
        };

        BillsStructureDto sut = usecases.GetBills(billsUID);

        Assert.NotNull(sut);
      }
    }


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
    public void Delete_Bill_Test() {

      TestsCommonMethods.Authenticate();

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        usecases.DeleteBill("09062604-3726-4c6b-96f5-c4a436b12626");

        Assert.True(true);
      }
    }


    [Fact]
    public void Get_Bill_Types_Test() {

      var sut = DocumentProduct.GetList<DocumentProduct>()
                     .FindAll(x => x.InternalCode.StartsWith("BILL-") &&
                              x.ApplicationContentType.Equals("complemento-pago-sat"))
                     .ToFixedList()
                     .Select(x => BillTypeDto.MapToBillTypeDto(x))
                     .ToFixedList();

      Assert.NotNull(sut);
    }

    #region Helpers

    #endregion Helpers


  } // class BillUseCasesTests

} // namespace Empiria.Tests.Billing
