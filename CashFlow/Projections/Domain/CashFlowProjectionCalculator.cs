/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Service provider                        *
*  Type     : CashFlowProjectionCalculator               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates cash flow projection entries with their values using financial data.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections {

  /// <summary>Generates cash flow projection entries with their values using financial data.</summary>
  internal class CashFlowProjectionCalculator {

    private CashFlowProjection _projection;

    internal CashFlowProjectionCalculator(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      _projection = projection;
    }


    internal FixedList<CashFlowProjectionEntryFields> CalculateEntries(string method) {
      Assertion.Require(method, nameof(method));

      return new FixedList<CashFlowProjectionEntryFields>();
    }


    internal FixedList<CashFlowProjectionEntry> GetCurrentEntries(string method) {
      Assertion.Require(method, nameof(method));

      return _projection.Entries.FindAll(x => x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("01") ||
                                              x.CashFlowAccount.StandardAccount.StdAcctNo.EndsWith("03"));
    }

  }  // class CashFlowProjectionCalculator

}  // namespace Empiria.CashFlow.Projections
