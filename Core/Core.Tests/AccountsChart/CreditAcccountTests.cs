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
using System;

using Empiria.Financial;
using Empiria.Financial.Accounts;

namespace Empiria.Tests.Financial.Accounts {

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
        Currency = 44,
        Interes = 20.4m,
        Comision = 3,
        Saldo = 1000.78m,
        PlazoInversion = 22,
        PlazoGracia = 7,
        PlazoAmortizacion = 1,
        Tasa = 32,
        FactorTasa = 2,
        TasaTecho = 12,
        FechaAmortizacion = DateTime.Now,
      };

      CreditAccount sut = TestsObjects.TryGetObject<CreditAccount>();

      sut.Update(fields);

      Assert.Equal(fields.EtapaCredito, sut.CreditData.EtapaCredito);
    }

    #endregion Facts

  }  // class CreditAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
