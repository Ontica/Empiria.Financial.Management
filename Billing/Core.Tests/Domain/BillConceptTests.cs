/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Test cases                              *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Unit tests                              *
*  Type     : BillConceptTests                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for BillConcept type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Billing;

namespace Empiria.Tests.Billing {

  /// <summary>Unit tests for BillConcept type.</summary>
  public class BillConceptTests {

    [Fact]
    public void Should_Parse_Empty_BillConcept() {
      var sut = BillConcept.Empty;

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Parse_All_Bill_Concepts() {
      var sut = BaseObject.GetFullList<BillConcept>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

  } // class BillConceptTests

} // namespace Empiria.Tests.Billing
