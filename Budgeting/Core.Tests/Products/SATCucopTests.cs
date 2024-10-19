/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Products                            Component : Test cases                              *
*  Assembly : Empiria.Budgeting.Core.Tests.dll           Pattern   : Unit tests                              *
*  Type     : SATCucopTests                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for SATCucop instances.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Budgeting.Products;

namespace Empiria.Tests.Budgeting.Products {

  /// <summary>Unit tests for SATCucop instances.</summary>
  public class SATCucopTests {

    [Fact]
    public void Should_Read_All_SATCucop_Instances() {
      var sut = SATCucop.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

  }  // class SATCucopTests

}  // namespace Empiria.Tests.Budgeting.Products
