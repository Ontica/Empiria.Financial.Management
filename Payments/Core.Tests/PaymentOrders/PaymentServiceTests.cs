/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Service tests                           *
*  Type     : PaymentServiceTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payment services.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System.Threading.Tasks;

using Empiria.Payments.Processor;
using Empiria.Payments;

namespace Empiria.Tests.Payments {

  /// <summary>Unit tests for payment services.</summary>
  public class PaymentServiceTests {

    #region Use cases initialization

    private readonly PaymentService _services;

    public PaymentServiceTests() {
      TestsCommonMethods.Authenticate();

      _services = PaymentService.ServiceInteractor();
    }


    ~PaymentServiceTests() {
      _services.Dispose();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public async Task Should_Send_Payment_Instruction() {
      var instruction = PaymentInstruction.Parse("fa3697b3-a42c-4753-b4bc-8d5f27737c4f");

      await _services.SendPaymentInstruction(instruction);

      Assert.True(true);
    }


    [Fact]
    public async Task UpdateInProgressPaymentInstructions() {
      await _services.UpdateInProgressPaymentInstructions();

      Assert.True(true);
    }

    #endregion Facts

  }  // class PaymentServiceTests

}  // namespace Empiria.Tests.Payments
