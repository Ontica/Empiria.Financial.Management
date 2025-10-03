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
using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Use cases used to retrieve cash flow plans.</summary>
  public class CashFlowReportsUseCases : UseCase {

    private readonly CashLedgerTotalsServices _accountingServices;

    #region Constructors and parsers

    protected CashFlowReportsUseCases() {
      _accountingServices = new CashLedgerTotalsServices();
    }

    static public CashFlowReportsUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowReportsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<DynamicDto<ConceptAnalyticsDto>> ConceptsAnalytics(CashFlowReportQuery query) {
      Assertion.Require(query, nameof(query));

      RecordsSearchQuery accountsTotalsQuery = MapToAccountsTotalsQuery(query);

      FixedList<ConceptAnalyticsDto> entries =
                        await _accountingServices.GetCashLedgerEntries<ConceptAnalyticsDto>(accountsTotalsQuery);

      return new DynamicDto<ConceptAnalyticsDto>(
        query,
        GetConceptsAnalyticColumns(),
        entries.Select(x => CompleteAnalytics(x)).ToFixedList()
      );
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


    private RecordsSearchQuery MapToAccountsTotalsQuery(CashFlowReportQuery query) {
      return new RecordsSearchQuery {
        QueryType = RecordSearchQueryType.None,
        FromDate = query.FromDate,
        ToDate = query.ToDate
      };
    }

    #endregion Helpers

  }  // class CashFlowReportsUseCases

}  // namespace Empiria.CashFlow.Explorer.UseCases
