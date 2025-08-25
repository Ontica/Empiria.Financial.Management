/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                  Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web Api Controller                    *
*  Type     : CashTransactionController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update transactions.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Financial.Reporting;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.UseCases;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Web API used to retrive and update cash ledger transactions.</summary>
  public class CashTransactionController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}/analyze")]
    public async Task<CollectionModel> AnalyzeCashTransaction([FromUri] long id) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionAnalysisEntry> analysisEntries = await usecases.AnalyzeTransaction(id);

        return new CollectionModel(base.Request, analysisEntries);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}")]
    public async Task<SingleObjectModel> GetCashTransaction([FromUri] long id) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        CashTransactionHolderDto transaction = await usecases.GetTransaction(id);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}/print")]
    public async Task<SingleObjectModel> GetCashTransactionAsPdfFile([FromUri] long id) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        CashTransactionHolderDto transaction = await usecases.GetTransaction(id);

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        FileDto pdfFile = reportingService.ExportTransactionToPdf(transaction);

        return new SingleObjectModel(base.Request, pdfFile);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/entries/search")]
    public async Task<CollectionModel> SearchCashEntries([FromBody] CashLedgerQuery query) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashEntryDescriptor> entries = await usecases.SearchEntries(query);

        return new CollectionModel(base.Request, entries);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/search")]
    public async Task<CollectionModel> SearchCashTransactions([FromBody] CashLedgerQuery query) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionDescriptor> transactions = await usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}/auto-codify")]
    public async Task<SingleObjectModel> AutoCodifyTransaction([FromUri] long id) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        CashTransactionHolderDto transaction = await usecases.AutoCodifyTransaction(id);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/auto-codify")]
    public async Task<SingleObjectModel> AutoCodifyTransactions([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        int count = await usecases.AutoCodifyTransactions(command.GetConvertedIds());

        var result = new {
          Message = $"Se auto codificaron {count} pólizas, de un total de {command.Items.Length} seleccionadas."
        };

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}/execute-command")]
    public async Task<SingleObjectModel> ExecuteCommand([FromUri] long id, [FromBody] CashEntriesCommand command) {

      command.TransactionId = id;

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        CashTransactionHolderDto transaction = await usecases.ExecuteCommand(command);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/export-entries")]
    public async Task<SingleObjectModel> ExportCashTransactionsToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionHolderDto> transactions = await usecases.GetTransactions(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new BulkOperationResult {
          File = reportingService.ExportTransactionsToExcel(transactions),
          Message = $"Se exportaron {transactions.Count} pólizas a Excel.",
        };

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/export-totals")]
    public async Task<SingleObjectModel> ExportCashTransactionsTotalsToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionHolderDto> transactions = await usecases.GetTransactions(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new BulkOperationResult {
          File = reportingService.ExportTransactionsTotalsToExcel(transactions),
          Message = $"Se exportaron {transactions.Count} pólizas a Excel.",
        };

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/entries/bulk-operation/export")]
    public async Task<SingleObjectModel> ExportCashEntriesToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashEntryDescriptor> entries = await usecases.GetTransactionsEntries(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new BulkOperationResult {
          File = reportingService.ExportTransactionsEntriesToExcel(entries),
          Message = $"Se exportaron {entries.Count} movimientos a Excel.",
        };

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Command web apis

  }  // class CashTransactionController

  public class BulkOperationCommand {

    public string[] Items {
      get; set;
    }


    internal FixedList<long> GetConvertedIds() {
      return Items.ToFixedList()
                  .Select(x => long.Parse(x))
                  .ToFixedList();
    }

  }  // class BulkOperationCommand


  public class BulkOperationResult {

    internal BulkOperationResult() {
      // no-op
    }

    public string Message {
      get; internal set;
    }

    public FileDto File {
      get; internal set;
    }

  }  // class BulkOperationResult

}  // namespace Empiria.CashFlow.WebApi
