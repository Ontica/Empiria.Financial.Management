/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillTaxEntryTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillTaxEntry type.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Billing;

namespace Empiria.Tests.Billing {

  /// <summary>Unit tests for BillTaxEntry type.</summary>
  public class BillTaxEntryTests {

    [Fact]
    public void Should_Parse_Empty_BillTaxEntry() {
      var sut = BillTaxEntry.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Parse_All_Bill_Concepts() {
      var sut = BaseObject.GetFullList<BillTaxEntry>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

  } // class BillTaxEntryTests

} // namespace Empiria.Tests.Billing
