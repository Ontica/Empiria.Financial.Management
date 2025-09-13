/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Use case interactor class               *
*  Type     : CashAccountUseCases                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrive cash accounts transactions.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.Financial.Adapters;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive cash accounts transactions.</summary>
  public class CashAccountUseCases : UseCase {

    private readonly CashTransactionServices _financialAccountingServices;

    #region Constructors and parsers

    protected CashAccountUseCases() {
      _financialAccountingServices = new CashTransactionServices();
    }

    static public CashAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<DynamicDto<CashAccountTotalDto>> SearchAccountsWithTotals(CashAccountTotalsQuery query) {
      Assertion.Require(query, nameof(query));

      var adaptedQuery = new CashAccountTotalsQuery {
        QueryType = query.QueryType,
        FromDate = query.FromDate,
        ToDate = query.ToDate,
        Accounts = query.Accounts,
        Ledgers = query.Ledgers,
        CustomFields = query.CustomFields,
      };

      FixedList<CashAccountTotalDto> accountsTotals =
                                    await _financialAccountingServices.GetCashAccountsTotals(adaptedQuery);

      FixedList<DataTableColumn> columns = new DataTableColumn[5] {
        new DataTableColumn("cashAccountNo", "CashAccountNo", "text"),
        new DataTableColumn("cashAccountName", "CashAccountName", "text"),
        new DataTableColumn("currencyCode", "CurrencyCode", "text"),
        new DataTableColumn("inflows", "Inflows", "decimal"),
        new DataTableColumn("outflows", "Outflows", "decimal"),
      }.ToFixedList();

      return new DynamicDto<CashAccountTotalDto>(query, columns, accountsTotals);
    }

    #endregion Use cases

  }  // class CashAccountUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
