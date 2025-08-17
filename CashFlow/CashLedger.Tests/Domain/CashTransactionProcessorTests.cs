/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Test cases                              *
*  Assembly : Empiria.CashFlow.CashLedger.Tests.dll      Pattern   : Unit tests                              *
*  Type     : CashTransactionProcessorTests              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for CashTransactionProcessor services.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.CashFlow.CashLedger;
using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.Tests.CashFlow.CashLedger {

  /// <summary>Unit tests for CashTransactionProcessor services.</summary>
  public class CashTransactionProcessorTests {

    #region Facts

    [Fact]
    public void Should_Execute() {
      var processor = new CashTransactionProcessor(new CashTransactionDescriptor(),
                                                   new FixedList<CashTransactionEntryDto>());

      FixedList<CashEntryFields> sut = processor.Execute();

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Validate_Constructor() {
      Assert.Throws<AssertionFailsException>(() => new CashTransactionProcessor(null, null));
      Assert.Throws<AssertionFailsException>(() => new CashTransactionProcessor(new CashTransactionDescriptor(), null));
    }

    #endregion Facts

  }  // class CashTransactionProcessorTests

}  // namespace Empiria.Tests.CashFlow.Projections
