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

namespace Empiria.Tests.Financial.Credits {

  /// <summary>Unit tests for credit accounts objects.</summary>
  public class CreditAcccountTests {

    #region Facts
    [Fact]
    public void Should_Update_CreditData() {
      var fields = new CreditExtDataFields {
        CreditoNo = "002000",
        EtapaCredito = 1,
        Acreditado = "Prueba",
        TipoCredito = "Linea directa",
       
      };

      CreditAccount sut = TestsObjects.TryGetObject<CreditAccount>();

      sut.Update(fields);

      Assert.Equal(fields.EtapaCredito, sut.CreditData.EtapaCredito);
    }


    [Fact]
    public void Should_Parse_All_Credit_Accounts() {
      var sut = BaseObject.GetFullList<CreditAccount>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);

    }
    #endregion Facts

  }  // class CreditAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
