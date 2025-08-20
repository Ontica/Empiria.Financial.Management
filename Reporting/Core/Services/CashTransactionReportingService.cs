/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : CashTransactionReportingService               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate cash transactions reports.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;
using System;

namespace Empiria.Financial.Reporting {

  /// <summary>Provides services used to generate cash transactions reports.</summary>
  public class CashTransactionReportingService : Service {

    #region Constructors and parsers

    private CashTransactionReportingService() {
      // no-op
    }

    static public CashTransactionReportingService ServiceInteractor() {
      return Service.CreateInstance<CashTransactionReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportTransactionToPdf(CashTransactionHolderDto transaction) {
      Assertion.Require(transaction, nameof(transaction));

      var templateUID = $"{this.GetType().Name}.ExportTransactionToPdf";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new CashTransactionVoucherBuilder(templateConfig);

      return exporter.CreateVoucher(transaction);
    }

    public FileDto ExportTransactionsToExcel(FixedList<CashTransactionHolderDto> transactions) {
      throw new NotImplementedException();
    }

    public FileDto ExportTransactionsEntriesToExcel(FixedList<CashEntryDescriptor> entries) {
      throw new NotImplementedException();
    }

    #endregion Services

  } // class CashTransactionReportingService

} // namespace Empiria.Financial.Reporting
