/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Test cases                              *
*  Assembly : Empiria.CashFlow.CashLedger.Tests.dll      Pattern   : Unit tests                              *
*  Type     : CashTransactionUseCasesTests               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashTransactionUseCases services.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using Empiria.CashFlow.CashLedger.UseCases;

namespace Empiria.Tests.CashFlow.CashLedger {

  /// <summary>Unit tests for CashTransactionUseCases services.</summary>
  public class CashTransactionUseCasesTests {

    private readonly CashTransactionUseCases _usecases;

    #region Constructors and parsers

    public CashTransactionUseCasesTests() {
      TestsCommonMethods.Authenticate();

      _usecases = CashTransactionUseCases.UseCaseInteractor();
    }

    ~CashTransactionUseCasesTests() {
      _usecases.Dispose();
    }

    #endregion Constructors and parsers

    #region Facts

    [Fact]
    public async void Should_AutoCodifyTransaction() {

      var sut = await _usecases.AutoCodifyTransaction(9114059);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class CashTransactionUseCasesTests

}  // namespace Empiria.Tests.CashFlow.Projections
