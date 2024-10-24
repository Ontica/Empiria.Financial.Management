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

using Empiria.Financial;

using Empiria.Payments.Payables;

namespace Empiria.Tests.Payments.Payables {

  /// <summary>Unit tests for payable objects.</summary>
  public class PayableTests {

    #region Facts

    [Fact]
    public void Should_Create_A_Payable() {
      PayableType payableType = PayableType.Parse(TestingConstants.PAYABLE_TYPE_UID);
      IPayableEntity payableEntity = payableType.ParsePayableEntity(TestingConstants.PAYABLE_ENTITY_UID);

      var sut = new Payable(payableType, payableEntity);

      Assert.Equal(payableType, sut.PayableType);
      Assert.Equal(payableEntity, sut.PayableEntity);
    }



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


    [Fact]
    public void Should_Parse_All_Payables_Payable_Entities() {
      var list = BaseObject.GetFullList<Payable>("PAYABLE_ENTITY_ID != -1");

      foreach (var payable in list) {
        var sut = payable.PayableEntity;

        Assert.True(sut.Id > 0);
        Assert.NotEmpty(sut.EntityNo);
      }
    }

    #endregion Facts

  }  // class PayableTests

}  // namespace Empiria.Tests.Payments.Payables
