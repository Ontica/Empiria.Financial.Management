/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : SATBillXmlReaderTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for SATBillXmlReader service.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Billing.SATMexicoImporter;

namespace Empiria.Tests.Billing {

  /// <summary>Unit tests for SATBillXmlReader service.</summary>
  public class SATBillXmlReaderTests {

    [Fact]
    public void Should_Read_A_Bill_From_A_Xml_String() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      var xmlText = System.IO.File.ReadAllText(xmlFilePath);

      var reader = new SATBillXmlReader(xmlText);

      SATBillDto sut = reader.ReadAsBillDto();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_A_Credit_Note_From_A_Xml_String() {

      string xmlFilePath = TestingConstants.XML_CREDIT_NOTE_FILE_PATH;

      var xmlText = System.IO.File.ReadAllText(xmlFilePath);

      var reader = new SATCreditNoteXmlReader(xmlText);

      SATBillDto sut = reader.ReadAsCreditNoteDto();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Read_A_Complement_Payment_From_A_Xml_String() {

      string xmlFilePath = TestingConstants.XML_PAYMENT_COMPLEMENT_FILE_PATH;

      var xmlText = System.IO.File.ReadAllText(xmlFilePath);

      var reader = new SATPaymentComplementXmlReader(xmlText);

      SatBillPaymentComplementDto sut = reader.ReadAsPaymentComplementDto();

      Assert.NotNull(sut);
    }

  }  // class SATBillXmlReaderTests

}  // namespace Empiria.Tests.Billing
