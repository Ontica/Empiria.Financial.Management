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

      PaymentOrder order = PaymentOrder.Parse("4954fdf9-b947-48f4-b8fa-610f559448d8");
      var sut = ExternalServices.SendPaymentOrderToPay(order);

       Assert.NotNull(sut);
    }
            

    [Fact]
    public void Should_Validate_PaymentInstruction_Is_Payed() {
      TestsCommonMethods.Authenticate();

      var sut = ExternalServices.ValidateIsPaymentInstructionPayed("2acdb601-af14-41a7-9f28-08fa7b42730e");

      Assert.NotNull(sut);
    }
        

    #endregion Facts

  }  // class PayableTests

}  // namespace Empiria.Tests.Payments
