/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Use case interactor class               *
*  Type     : CashFlowExplorerUseCases                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow explorer information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;

using Empiria.CashFlow.Explorer.Adapters;
using System;

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

    public Task<CashFlowExplorerResultDto> ExploreCashFlow(CashFlowExplorerQuery query) {
      Assertion.Require(query, nameof(query));

      switch (query.ReportType) {
        case CashFlowReportType.CashFlow:
          return GetCashFlowReport(query);

        case CashFlowReportType.ConceptAnalytic:
          return GetConceptAnalyticReport(query);

        default:
          throw Assertion.EnsureNoReachThisCode($"El reporte {query.ReportType} no está disponible.");
      }
    }

    #endregion Use cases

    #region Helpers

    private async Task<CashFlowExplorerResultDto> GetCashFlowReport(CashFlowExplorerQuery query) {
      FixedList<CashLedgerTotalEntryDto> totals = await _financialAccountingServices.GetCashLedgerTotals(query);

      var explorer = new CashFlowExplorer(query, totals);

      CashFlowExplorerResult result = explorer.Execute();

      return CashFlowExplorerResultMapper.Map(result);
    }


    private async Task<CashFlowExplorerResultDto> GetConceptAnalyticReport(CashFlowExplorerQuery query) {
      return await Task.FromException<CashFlowExplorerResultDto>(new NotImplementedException());
    }

    #endregion Helpers

  }  // class CashFlowExplorerUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
