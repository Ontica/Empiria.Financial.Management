/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Service provider                        *
*  Type     : CashFlowSearchServices                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to retrieve cash flow related data and entities.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.Financial;
using Empiria.Financial.Concepts;

using Empiria.Financial.Adapters;
using Empiria.CashFlow.Explorer.Adapters;

namespace Empiria.CashFlow.Explorer.UseCases {

  /// <summary>Provides services used to retrieve cash flow related data and entities.</summary>
  public class CashFlowSearchServices : Service {

    #region Constructors and parsers

    protected CashFlowSearchServices() {
      // no-op
    }

    static public CashFlowSearchServices ServiceInteractor() {
      return CreateInstance<CashFlowSearchServices>();
    }

    #endregion Constructors and parsers

    #region Services

    public DynamicDto<CashFlowAccountDto> SearchCashFlowConcepts(RecordsSearchQuery query) {
      Assertion.Require(query, nameof(query));

      var accounts = FinancialAccount.GetList()
                                     .FindAll(a => a.IsOperationAccount);

      if (query.ClassificationUID.Length != 0) {
        var concept = FinancialConcept.Parse(query.ClassificationUID);

        accounts = accounts.FindAll(a => a.StandardAccount.MainClassification.ConceptNo.StartsWith(concept.ConceptNo));
      }

      if (query.OperationTypeUID.Length != 0) {
        accounts = accounts.FindAll(a => a.OperationType.UID == query.OperationTypeUID);
      }

      if (query.OrgUnitUID.Length != 0) {
        accounts = accounts.FindAll(a => a.OrganizationalUnit.UID == query.OrgUnitUID);
      }

      var dtos = accounts.ConvertAll(a => new CashFlowAccountDto {
        CashAccountNo = a.AccountNo,
        CashAccountName = a.Description,
        OperationType = $"({a.OperationType.Code}) {a.StandardAccount.Description}",
        FinancialAccountName = $"({a.Parent.Code}) {a.Parent.Name}",
        OrgUnitName = a.OrganizationalUnit.FullName,
        CurrencyCode = a.Currency.ISOCode
      }).ToFixedList();

      dtos = dtos.OrderBy(x => x.CashAccountNo)
                 .ThenBy(x => x.FinancialAccountName)
                 .ThenBy(x => x.OrgUnitName)
                 .ThenBy(x => x.OperationType)
                 .ThenBy(x => x.CurrencyCode)
                 .ToFixedList();

      var columns = new DataTableColumn[] {
        new DataTableColumn("cashAccountNo", "Concepto", "text"),
        new DataTableColumn("cashAccountName", "Nombre", "text"),
        new DataTableColumn("financialAccountName", "Cuenta", "text"),
        new DataTableColumn("orgUnitName", "Área", "text"),
        new DataTableColumn("operationType", "Tipo de operación", "text"),
        new DataTableColumn("currencyCode", "Moneda", "text"),
      }.ToFixedList();

      return new DynamicDto<CashFlowAccountDto>(query, columns, dtos);
    }


    public DynamicDto<ICreditEntryData> SearchExternalCreditEntries(RecordsSearchQuery query) {
      Assertion.Require(query, nameof(query));

      ExternalCreditSystemServices provider = new ExternalCreditSystemServices();

      FixedList<ICreditEntryData> entries = provider.GetCreditEntries(query.Keywords.ToFixedList(),
                                                                      query.FromDate,
                                                                      query.ToDate);
      var columns = new DataTableColumn[] {
        new DataTableColumn("accountNo", "No crédito", "text"),
        new DataTableColumn("subledgerAccountNo", "Auxiliar", "text"),
        new DataTableColumn("applicationDate", "Fecha", "date"),
        new DataTableColumn("operationTypeNo", "Clave", "text"),
        new DataTableColumn("operationName", "Concepto", "text"),
        new DataTableColumn("amount", "Importe", "decimal"),
      }.ToFixedList();

      return new DynamicDto<ICreditEntryData>(query, columns, entries);
    }

    #endregion Services

  }  // class Services

}  // namespace Empiria.CashFlow.Explorer.UseCases
