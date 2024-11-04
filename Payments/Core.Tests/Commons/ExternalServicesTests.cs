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

    #endregion Facts

  }  // class PayableTests

}  // namespace Empiria.Tests.Payments
