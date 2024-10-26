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
    public void Should_Read_A_Bill_From_An_Xml_File() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      var reader = new SATBillXmlReader(xmlFilePath);

      SATBillDto sut = reader.ReadAsBillDto();

      Assert.NotNull(sut);
    }

  }  // class SATBillXmlReaderTests

}  // namespace Empiria.Tests.Billing
