/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Use case tests                          *
*  Type     : BillImporterUseCaseTests                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use case test for import bills from Xml structures using SAT Mexico standard.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

namespace Empiria.Tests.Billing {

  ///<summary>Use cases test for import bills from Xml structures using SAT Mexico standard.</summary>
  public class BillImporterUseCaseTests {

    [Fact]
    public void Should_Import_A_Bill_From_An_Xml_File() {

      using (var usecase = BillImporterUseCases.UseCaseInteractor()) {

        string xmlFilePath = TestingConstants.XML_BILL_FILE_PATH;

        BillDto sut = usecase.ImportXmlBillFromFilePath(xmlFilePath);

        Assert.NotNull(sut);
      }
    }

  }  // class BillImporterUseCaseTests

}  // namespace Empiria.Tests.Billing
