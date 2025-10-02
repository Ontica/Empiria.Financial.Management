/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Use case interactor class               *
*  Type     : CashFlowAccountsTotalsUseCases             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow related accounts information.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.Financial;
using Empiria.Financial.Adapters;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Use cases used to retrieve cash flow related accounts information.</summary>
  public class CashFlowAccountsTotalsUseCases : UseCase {

    private readonly CashLedgerTotalsServices _accountingServices;

    #region Constructors and parsers

    protected CashFlowAccountsTotalsUseCases() {
      _accountingServices = new CashLedgerTotalsServices();
    }

    static public CashFlowAccountsTotalsUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowAccountsTotalsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<DynamicDto<CashAccountTotalDto>> AccountTotals(RecordsSearchQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<CashAccountTotalDto> accountsTotals =
                                            await _accountingServices.GetCashAccountTotals(query);

      foreach (var total in accountsTotals) {
        if (total.CashAccountId <= 0) {
          continue;
        }
        total.CashAccountName = FinancialAccount.Parse(total.CashAccountId).Description;
      }

      var columns = new DataTableColumn[5] {
        new DataTableColumn("cashAccountNo", "CashAccountNo", "text"),
        new DataTableColumn("cashAccountName", "CashAccountName", "text"),
        new DataTableColumn("currencyCode", "CurrencyCode", "text"),
        new DataTableColumn("inflows", "Debit", "decimal"),
        new DataTableColumn("outflows", "Credit", "decimal"),
      }.ToFixedList();

      return new DynamicDto<CashAccountTotalDto>(query, columns, accountsTotals);
    }

    #endregion Use cases

  }  // class CashFlowAccountsTotalsUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
