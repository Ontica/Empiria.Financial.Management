/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Information holder                      *
*  Type     : CashFlowTotalEntry                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds an entry for the CashFlowTotalsReport.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Concepts;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Holds an entry for the CashFlowTotalsReport.</summary>
  public class CashFlowTotalEntry {

    private readonly FinancialConcept _concept;

    internal CashFlowTotalEntry(FinancialConcept concept) {
      _concept = concept;
    }

    internal FinancialConcept Concept {
      get {
        return _concept;
      }
    }

    public string ConceptNo {
      get {
        return _concept.ConceptNo;
      }
    }

    public string ConceptName {
      get {
        return _concept.Name;
      }
    }

    public decimal Inflows {
      get; private set;
    }

    public decimal Outflows {
      get; private set;
    }

    public decimal Total {
      get {
        if (ConceptNo.StartsWith("1")) {
          return Inflows;

        } else if (ConceptNo.StartsWith("2")) {
          return Outflows;

        } else {
          return Inflows - Outflows;
        }
      }
    }


    internal void Sum(CashFlowExplorerEntry entry) {
      if (ConceptNo.StartsWith("1")) {
        Inflows += entry.Inflows;

      } else if (ConceptNo.StartsWith("2")) {
        Outflows += entry.Outflows;

      } else {
        Inflows += entry.Inflows;
        Outflows += entry.Outflows;
      }
    }


    internal void SumValuated(CashFlowExplorerEntry entry) {
      if (ConceptNo.StartsWith("1")) {
        Inflows += Valuate(entry.CurrencyCode, entry.Inflows);

      } else if (ConceptNo.StartsWith("2")) {
        Outflows += Valuate(entry.CurrencyCode, entry.Outflows);

      } else {
        Inflows += Valuate(entry.CurrencyCode, entry.Inflows);
        Outflows += Valuate(entry.CurrencyCode, entry.Outflows);
      }
    }


    private decimal Valuate(string currencyCode, decimal amount) {
      switch (currencyCode) {
        case "MXN":
          return amount;

        case "UDI":
          return amount * 8.525333m;

        case "USD":
          return amount * 18.868000m;

        case "EUR":
          return amount * 21.596310m;

        case "JPY":
          return amount * 0.125470m;

        default:
          return amount;
      }
    }

  }  // class CashFlowTotalEntry

}  // namespace Empiria.CashFlow.Explorer
