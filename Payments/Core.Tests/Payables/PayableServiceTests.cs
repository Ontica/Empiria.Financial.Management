/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Service tests                           *
*  Type     : PayableServiceTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for payable service.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Payments.Payables;
using Empiria.Payments.Payables.Services;

namespace Empiria.Tests.Payments.Payables.Services {

  /// <summary>Test cases for payable service.</summary>
  public class PayableServiceTests {
     
    #region Facts

    [Fact]
    public void Should_Set_OnPayment() {

      var payable = Payable.Parse("abdc27b9-5fb1-4386-aa87-f5ad5ec66fea");

      PayableServices.SetOnPayment(payable);
    }


    [Fact]
    public void Should_Set_Pay() {

      var payable = Payable.Parse("abdc27b9-5fb1-4386-aa87-f5ad5ec66fea");

      PayableServices.SetAsPayed(payable);
    }

    #endregion Facts

  }  //  class  PayableServiceUseCasesTests

}  // namespace Empiria.Tests.Payments.Payables.Services
