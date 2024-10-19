/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillXmlReaderTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillXmlReader service.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Billing;
using Empiria.Billing.Adapters;

namespace Empiria.Tests.Billing {

  /// <summary>Unit tests for BillXmlReader service.</summary>
  public class BillXmlReaderTests {

    [Fact]
    public void Should_Read_A_Bill_From_An_Xml_File() {

      string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

      BillDto sut = BillXmlReader.ReadFromFilePath(xmlFilePath);

      Assert.NotNull(sut);
    }

  }  // class BillXmlReaderTests

}  // namespace Empiria.Tests.Billing
