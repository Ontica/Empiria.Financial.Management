/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : PayableTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payable objects.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Payments.Payables;

namespace Empiria.Tests.Payments.Payables {

  /// <summary>Unit tests for payable objects.</summary>
  public class PayableTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Payables() {
      var sut = BaseObject.GetFullList<Payable>();

      Assert.NotNull(sut);
      Assert.True(sut.Count > 0);
    }


    [Fact]
    public void Should_Read_Empty_Payable() {
      var sut = Payable.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class PayableTests

}  // namespace Empiria.Tests.Payments.Payables
