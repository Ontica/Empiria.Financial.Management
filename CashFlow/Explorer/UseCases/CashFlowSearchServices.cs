/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Service provider                        *
*  Type     : CashFlowSearchServices                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to retrieve cash flow related data and entities.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;
using Empiria.Services;

using Empiria.Financial;
using Empiria.Financial.Adapters;

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

    public DynamicDto<ICreditEntryData> SearchExternalCreditEntries(RecordsSearchQuery query) {

      ExternalCreditSystemServices provider = new ExternalCreditSystemServices();

      FixedList<ICreditEntryData> entries = provider.GetCreditEntries(query.Keywords.ToFixedList(),
                                                                        query.FromDate,
                                                                        query.ToDate);
      var columns = new DataTableColumn[6] {
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
