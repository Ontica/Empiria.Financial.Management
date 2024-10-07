/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : PayableItemTests                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payable items objects.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Payments.Payables;

namespace Empiria.Tests.Payments.Payables {

  /// <summary>Unit tests for payable items objects.</summary>
  public class PayableItemTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Payable_Items() {
      var sut = BaseObject.GetFullList<PayableItem>();

      Assert.NotNull(sut);
      Assert.True(sut.Count > 0);
    }


    [Fact]
    public void Should_Read_Empty_PayableItem() {
      var sut = PayableItem.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class PayableItemTests

}  // namespace Empiria.Tests.Payments.Payables
