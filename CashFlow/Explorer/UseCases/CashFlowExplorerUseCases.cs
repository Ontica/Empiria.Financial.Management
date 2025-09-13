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

using Empiria.Financial;
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

    public async Task<DynamicDto<CashAccountTotalDto>> AccountTotals(CashAccountTotalsQuery query) {
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
                                    await _accountingServices.GetCashAccountTotals(adaptedQuery);

      FixedList<DataTableColumn> columns = new DataTableColumn[5] {
        new DataTableColumn("cashAccountNo", "CashAccountNo", "text"),
        new DataTableColumn("cashAccountName", "CashAccountName", "text"),
        new DataTableColumn("currencyCode", "CurrencyCode", "text"),
        new DataTableColumn("inflows", "Inflows", "decimal"),
        new DataTableColumn("outflows", "Outflows", "decimal"),
      }.ToFixedList();

      return new DynamicDto<CashAccountTotalDto>(query, columns, accountsTotals);
    }


    public async Task<DynamicDto<ConceptAnalyticsDto>> ConceptsAnalytics(CashFlowExplorerQuery query) {
      FixedList<ConceptAnalyticsDto> entries =
                        await _accountingServices.GetCashLedgerEntries<ConceptAnalyticsDto>(query);

      return new DynamicDto<ConceptAnalyticsDto>(
        query,
        GetConceptsAnalyticColumns(),
        entries.Select(x => CompleteAnalytics(x)).ToFixedList()
      );
    }

    public async Task<DynamicDto<CashFlowExplorerEntry>> ExploreCashFlow(CashFlowExplorerQuery query) {
      FixedList<CashAccountTotalDto> totals = await _accountingServices.GetCashAccountTotals(query);

      var explorer = new CashFlowExplorer(query, totals);

      DynamicDto<CashFlowExplorerEntry> result = explorer.Execute();

      return result;
    }

    #endregion Use cases

    #region Helpers

    private ConceptAnalyticsDto CompleteAnalytics(ConceptAnalyticsDto dto) {

      if (dto.CashAccountId <= 0) {
        return dto;
      }

      FinancialAccount cashAccount = FinancialAccount.Parse(dto.CashAccountId);

      dto.ConceptDescription = cashAccount.Description;
      dto.FinancialAcctName = cashAccount.Parent.Name;
      dto.FinancialAcctOrgUnit = cashAccount.OrganizationalUnit.FullName;
      dto.FinancialAcctType = cashAccount.Parent.FinancialAccountType.DisplayName;
      dto.ProjectType = cashAccount.Parent.Project.Category.Name;

      return dto;
    }


    private FixedList<DataTableColumn> GetConceptsAnalyticColumns() {
      return new List<DataTableColumn> {
        new DataTableColumn("cashAccountNo", "Concepto presupuestal", "text"),
        new DataTableColumn("conceptDescription", "Descripción", "text"),
        new DataTableColumn("accountNumber", "Cuenta", "text"),
        new DataTableColumn("transactionAccountingDate", "Fecha", "date"),
        new DataTableColumn("currencyName", "Mon", "text"),
        new DataTableColumn("exchangeRate", "T.Cambio", "decimal"),
        new DataTableColumn("debit", "Cargo", "decimal"),
        new DataTableColumn("credit", "Abono", "decimal"),
        new DataTableColumn("transactionNumber", "Póliza", "text-link"),
        new DataTableColumn("financialAcctOrgUnit", "Área", "text"),
        new DataTableColumn("subledgerAccountNumber", "Auxiliar", "text"),
        new DataTableColumn("projectType", "Clave de obra", "text"),
        new DataTableColumn("financialAcctType", "Tipo de cuenta", "text"),
        new DataTableColumn("financialAcctName", "Nombre de la cuenta", "text"),
      }.ToFixedList();
    }

    #endregion Helpers

  }  // class CashFlowExplorerUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
