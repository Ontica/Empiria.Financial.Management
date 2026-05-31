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
using Empiria.Json;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for credit accounts objects.</summary>
  public class CreditAcccountTests {

    #region Facts
    [Fact]
    public void Should_Update_CreditData() {
      var json = new JsonObject() {
        { "creditTypeUID", TestsObjects.TryGetObject<CreditType>().UID },
        { "creditRiskStageUID", TestsObjects.TryGetObject<CreditRiskStage>().UID },
        { "creditProcessStageUID", TestsObjects.TryGetObject<CreditProcessStage>().UID},
        { "borrower", "La Vía Óntica SC" },
        { "externalCreditNo", EmpiriaString.BuildRandomString(32) }
      };

      var attributes = new CreditAttributes(json);

      var fields = new FinancialAccountFields {
        Attributes = attributes,
      };

      FinancialAccount sut = TestsObjects.TryGetObject<FinancialAccount>();

      sut.Update(fields);

      var sutAttributes = (CreditAttributes) sut.Attributes;

      Assert.Equal(attributes.CreditType, sutAttributes.CreditType);
      Assert.Equal(attributes.CreditRiskStage, sutAttributes.CreditRiskStage);
      Assert.Equal(attributes.CreditProcessStage, sutAttributes.CreditProcessStage);
      Assert.Equal(attributes.Borrower, sutAttributes.Borrower);
      Assert.Equal(attributes.ExternalCreditNo, sutAttributes.ExternalCreditNo);
    }


    #endregion Facts

  }  // class CreditAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
