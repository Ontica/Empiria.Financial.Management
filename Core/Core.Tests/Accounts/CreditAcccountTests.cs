/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : CreditAcccountTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for credit accounts objects.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for credit accounts objects.</summary>
  public class CreditAcccountTests {

    #region Facts
    [Fact]
    public void Should_Update_CreditData() {
      var attributes = new CreditAttributes {
        Borrower = "La Vía Óntica SC",
        CreditStage = TestsObjects.TryGetObject<CreditStage>(),
        CreditType = TestsObjects.TryGetObject<CreditType>(),
        ExternalCreditNo = EmpiriaString.BuildRandomString(32)
      };

      var fields = new FinancialAccountFields {
        Attributes = attributes,
      };

      FinancialAccount sut = TestsObjects.TryGetObject<FinancialAccount>();

      sut.Update(fields);

      Assert.Equal(((CreditAttributes) fields.Attributes).Borrower, ((CreditAttributes) sut.Attributes).Borrower);
      Assert.Equal(((CreditAttributes) fields.Attributes).CreditStage, ((CreditAttributes) sut.Attributes).CreditStage);
      Assert.Equal(((CreditAttributes) fields.Attributes).CreditType, ((CreditAttributes) sut.Attributes).CreditType);
      Assert.Equal(((CreditAttributes) fields.Attributes).ExternalCreditNo, ((CreditAttributes) sut.Attributes).ExternalCreditNo);
    }


    #endregion Facts

  }  // class CreditAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
