/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Report Builder                          *
*  Type     : CashFlowTotalsReport                      License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Builds a cash flow totals report according to a cash flow accounts tree.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.Financial.Concepts;

using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer {

  /// <summary>Builds a cash flow totals report according to a cash flow accounts tree.</summary>
  internal class CashFlowTotalsReport {

    private readonly CashFlowExplorerQuery _query;
    private readonly FixedList<CashFlowExplorerEntry> _sourceEntries;

    private List<CashFlowTotalEntry> entries = new List<CashFlowTotalEntry>(128);

    public CashFlowTotalsReport(CashFlowExplorerQuery query,
                                FixedList<CashFlowExplorerEntry> sourceEntries) {
      _query = query;
      _sourceEntries = sourceEntries;

    }


    internal void Build() {
      FillBaseFinancialConceptEntries();
      SumSourceEntries();
      SumParentEntries();
    }


    internal DynamicDto<CashFlowTotalEntry> ToDynamicDto() {
      return new DynamicDto<CashFlowTotalEntry>(_query, GetColumns(), entries.ToFixedList());
    }

    #region Builders

    private void FillBaseFinancialConceptEntries() {
      FixedList<FinancialConcept> concepts = FinancialConceptGroup.ParseWithNamedKey("CASH_FLOW")
                                                                  .GetConcepts();

      foreach (var concept in concepts) {
        var entry = new CashFlowTotalEntry(concept);
        entries.Add(entry);
      }
    }


    private void SumParentEntries() {
      foreach (var sourceEntry in _sourceEntries) {
        FinancialConcept concept = sourceEntry.MainClassification;

        FinancialConcept parent = concept.Parent;

        while (!parent.IsEmptyInstance) {

          var entry = entries.Find(e => e.ConceptNo == parent.ConceptNo);

          if (entry != null) {
            entry.Sum(sourceEntry);
          }

          parent = parent.Parent;
        }  // while

      } // foreach
    }


    private void SumSourceEntries() {
      foreach (var sourceEntry in _sourceEntries) {
        FinancialConcept concept = sourceEntry.MainClassification;

        var entry = entries.Find(e => e.ConceptNo == concept.ConceptNo);

        if (entry == null) {
          continue;
        }

        entry.SumValuated(sourceEntry);
      }
    }

    #endregion Builders

    #region Helpers

    private FixedList<DataTableColumn> GetColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("conceptNo", "Cuenta", "text"),
        new DataTableColumn("conceptName", "Descripción", "text"),
        new DataTableColumn("inflows", "Entradas", "decimal"),
        new DataTableColumn("outflows", "Salidas", "decimal"),
        new DataTableColumn("total", "Total", "decimal"),
      }.ToFixedList();
    }

    #endregion Helpers

  }  // class CashFlowTotalsReport

}  // namespace Empiria.CashFlow.Explorer
