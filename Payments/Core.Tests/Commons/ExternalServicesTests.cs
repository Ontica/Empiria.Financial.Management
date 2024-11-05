/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : ExternalServicesTests                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payments ExternalServices.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Payments;
using Empiria.Payments.Payables;
using Empiria.Payments.Orders;

namespace Empiria.Tests.Payments {

  /// <summary>Unit tests for payments ExternalServices.</summary>
  public class ExternalServicesTests {

    #region Facts

    [Fact]
    public void Should_Commit_Budget() {
      TestsCommonMethods.Authenticate();

      Payable sut = Payable.Parse(TestingConstants.PAYABLE_ID);

      ExternalServices.CommitBudget(sut);
    }


    [Fact]
    public void Should_Exercise_Budget() {
      TestsCommonMethods.Authenticate();

      Payable sut = Payable.Parse(TestingConstants.PAYABLE_ID);

      ExternalServices.ExerciseBudget(sut);
    }


    [Fact]
    public void Should_Send_PaymentOrder_ToPay() {
      TestsCommonMethods.Authenticate();

      PaymentOrder order = PaymentOrder.Parse("ae03e327-7384-44d7-91b4-48ee6d47d3d4");
      var sut = ExternalServices.SendPaymentOrderToPay(order);

       Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class PayableTests

}  // namespace Empiria.Tests.Payments
