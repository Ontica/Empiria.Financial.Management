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
        NoCredito = "002000",
        EtapaCredito = 1,
        Acreditado = "La Vía Óntica SC",
        TipoCredito = "Linea directa",
      };

      var fields = new FinancialAccountFields {
        Attributes = attributes,
      };

      FinancialAccount sut = TestsObjects.TryGetObject<FinancialAccount>();

      sut.Update(fields);

      Assert.Equal(((CreditAttributes) fields.Attributes).Acreditado, ((CreditAttributes) sut.Attributes).Acreditado);
      Assert.Equal(((CreditAttributes) fields.Attributes).EtapaCredito, ((CreditAttributes) sut.Attributes).EtapaCredito);
    }


    #endregion Facts

  }  // class CreditAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
