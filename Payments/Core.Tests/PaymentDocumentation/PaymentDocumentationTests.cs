/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Service tests                           *
*  Type     : PaymentDocumentationTests                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for payment documentation services.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using Empiria.Payments;
using System;

namespace Empiria.Tests.Payments {

  /// <summary>Unit tests for payment documentation services.</summary>
  public class PaymentDocumentationTests {

    #region Use cases initialization

    public PaymentDocumentationTests() {
      TestsCommonMethods.Authenticate();
    }

    #endregion Use cases initialization

    #region Facts

    [Fact]
    public void Should_Create_PaymentDocuments_Directory() {
      DateTime fromDate = Convert.ToDateTime("15-01-2026");
      DateTime toDate = Convert.ToDateTime("11-02-2026");

      var sut = PaymentDocumentation.Proccess(fromDate, toDate);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class PaymentDocumentationTests

}  // namespace Empiria.Tests.Payments
