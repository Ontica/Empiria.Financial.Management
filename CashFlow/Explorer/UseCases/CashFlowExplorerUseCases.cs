/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Use case interactor class               *
*  Type     : CashFlowExplorerUseCases                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow explorer information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Use cases used to retrieve cash flow plans.</summary>
  public class CashFlowExplorerUseCases : UseCase {

    private readonly CashLedgerTotalsServices _financialAccountingServices;

    #region Constructors and parsers

    protected CashFlowExplorerUseCases() {
      _financialAccountingServices = new CashLedgerTotalsServices();
    }

    static public CashFlowExplorerUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowExplorerUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<DynamicDto<CashEntryDescriptor>> ConceptsAnalytics(CashFlowExplorerQuery query) {
      FixedList<CashEntryDescriptor> entries = await _financialAccountingServices.GetCashLedgerEntries(query);

      var result = new DynamicResult<CashEntryDescriptor> {
        Query = query,
        Columns = GetConceptsAnalyticColumns(),
        Entries = entries
      };

      return CashFlowExplorerResultMapper.Map(result);
    }

    public async Task<DynamicDto<CashFlowExplorerEntry>> ExploreCashFlow(CashFlowExplorerQuery query) {
      FixedList<CashLedgerTotalEntryDto> totals = await _financialAccountingServices.GetCashLedgerTotals(query);

      var explorer = new CashFlowExplorer(query, totals);

      DynamicResult<CashFlowExplorerEntry> result = explorer.Execute();

      return CashFlowExplorerResultMapper.Map(result);
    }

    #endregion Use cases

    #region Helpers

    private FixedList<DataTableColumn> GetConceptsAnalyticColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("cashAccountNo", "Concepto presupuestal", "text"),
        new DataTableColumn("conceptDescription", "Descripción", "text"),
        new DataTableColumn("program", "Programa", "text"),
        new DataTableColumn("subprogram", "Subprograma", "text"),
        new DataTableColumn("financingSource", "Fuente", "text"),
        new DataTableColumn("operationType", "Operación", "text"),
        new DataTableColumn("currencyCode", "Moneda", "text"),
        new DataTableColumn("inflows", "Entradas", "decimal"),
        new DataTableColumn("outflows", "Salidas", "decimal"),
      }.ToFixedList();
    }

    #endregion Helpers

  }  // class CashFlowExplorerUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
