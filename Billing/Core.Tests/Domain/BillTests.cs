/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillTests                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Bill type.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Billing;

namespace Empiria.Tests.Billing {

  /// <summary>Unit tests for Bill type.</summary>
  public class BillTests {

    [Fact]
    public void Should_Parse_Empty_Bill() {
      var sut = Bill.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Parse_All_Bills() {
      var sut = Bill.GetFullList<Bill>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

  } // class BillTests

} // namespace Empiria.Tests.Billing
