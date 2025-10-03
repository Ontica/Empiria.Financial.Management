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

using Empiria.DynamicData;
using Empiria.Services;
using Empiria.Financial.Adapters;
using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Use cases used to retrieve cash flow plans.</summary>
  public class CashFlowExplorerUseCases : UseCase {

    private readonly CashLedgerTotalsServices _accountingServices;

    #region Constructors and parsers

    protected CashFlowExplorerUseCases() {
      _accountingServices = new CashLedgerTotalsServices();
    }

    static public CashFlowExplorerUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowExplorerUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<DynamicDto<CashFlowExplorerEntry>> ExploreCashFlow(CashFlowExplorerQuery query) {
      Assertion.Require(query, nameof(query));

      RecordsSearchQuery accountsTotalsQuery = MapToAccountsTotalsQuery(query);

      FixedList<CashAccountTotalDto> totals =
                                        await _accountingServices.GetCashAccountTotals(accountsTotalsQuery);

      var explorer = new CashFlowExplorer(query, totals);

      return explorer.Execute();
    }

    #endregion Use cases

    #region Helpers

    private RecordsSearchQuery MapToAccountsTotalsQuery(CashFlowExplorerQuery query) {
      return new RecordsSearchQuery {
        QueryType = RecordSearchQueryType.None,
        FromDate = query.FromDate,
        ToDate = query.ToDate
      };
    }

    #endregion Helpers

  }  // class CashFlowExplorerUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
