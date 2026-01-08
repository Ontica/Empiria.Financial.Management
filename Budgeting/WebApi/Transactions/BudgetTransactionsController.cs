/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web Api Controller                    *
*  Type     : BudgetTransactionsController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and edit budget transactions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;
using System.Web.Http;

using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.WebApi;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Budgeting.Reporting;

namespace Empiria.Budgeting.Transactions.WebApi {

  /// <summary>Web API used to retrieve and edit budget transactions.</summary>
  public class BudgetTransactionsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/authorize")]
    public SingleObjectModel AuthorizeTransaction([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.AuthorizeTransaction(budgetTransactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/close")]
    public SingleObjectModel CloseTransaction([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.CloseTransaction(budgetTransactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions")]
    public SingleObjectModel CreateTransaction([FromBody] BudgetTransactionFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.CreateTransaction(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}")]
    public NoDataModel DeleteOrCancelTransaction([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteOrCancelTransaction(budgetTransactionUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/bulk-operation/export-entries-grouped")]
    public SingleObjectModel ExportGroupedEntriesToExcel([FromBody] BulkOperationCommand command) {

      FixedList<BudgetTransactionByYear> transactions =
            command.Items.Select(x => new BudgetTransactionByYear(BudgetTransaction.Parse(x)))
                         .ToFixedList()
                         .Sort((x, y) => x.Transaction.TransactionNo.CompareTo(y.Transaction.TransactionNo));

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportGroupedEntriesToExcel(transactions),
            $"Se exportaron {transactions.Count} transacciones presupuestales a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/bulk-operation/export-entries-ungrouped")]
    public SingleObjectModel ExportUngroupedEntriesToExcel([FromBody] BulkOperationCommand command) {

      FixedList<BudgetTransaction> transactions =
            command.Items.Select(x => BudgetTransaction.Parse(x))
                         .ToFixedList()
                         .Sort((x, y) => x.TransactionNo.CompareTo(y.TransactionNo));

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportUngroupedEntriesToExcel(transactions),
            $"Se exportaron {transactions.Count} transacciones presupuestales a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }

    [HttpPost]
    [Route("v2/budgeting/transactions/planning/generate")]
    public SingleObjectModel GeneratePlanningTransactions() {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

        int closed = usecases.AutoCloseTransactions(budget);

        var result = new {
          Message = $"Se cerraron automáticamente {closed} transacciones presupuestales para el presupuesto {budget.Name}."
        };

        return new SingleObjectModel(base.Request, result);
      }

      //using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

      //  Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

      //  FixedList<BudgetTransactionDescriptorDto> generated = usecases.GeneratePlanningTransactions(budget.UID);

      //  var result = new BulkOperationResult {
      //    Message = $"Se generaron {generated.Count} transacciones presupuestales para el presupuesto {budget.Name}."
      //  };

      //  return new SingleObjectModel(base.Request, result);
      //}
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/operation-sources")]
    public SingleObjectModel GetOperationSources() {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> operationSources = usecases.GetOperationSources();

        return new SingleObjectModel(base.Request, operationSources);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{transactionUID:guid}")]
    public SingleObjectModel GetTransaction([FromUri] string transactionUID) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.GetTransaction(transactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/print")]
    public SingleObjectModel PrintTransaction([FromUri] string transactionUID) {

      var transaction = BudgetTransaction.Parse(transactionUID);

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        FileDto file = reportingService.ExportTransactionVoucherToPdf(transaction);

        return new SingleObjectModel(base.Request, file);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/reject")]
    public SingleObjectModel RejectTransaction([FromUri] string transactionUID,
                                               [FromBody] RejectFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.RejectTransaction(transactionUID,
                                                                            fields.Message);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/accounts/search")]
    public CollectionModel SearchTransactionAccounts([FromBody] BudgetAccountsQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<BudgetAccountDto> accounts = usecases.SearchBudgetAccounts(query);

        return new CollectionModel(base.Request, accounts);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/search")]
    public CollectionModel SearchTransactions([FromBody] BudgetTransactionsQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<BudgetTransactionDescriptorDto> transactions = usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/parties")]
    public CollectionModel SearchTransactionsParties([FromBody] TransactionPartiesQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> parties = usecases.SearchTransactionsParties(query);

        return new CollectionModel(base.Request, parties);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/send-to-authorization")]
    public SingleObjectModel SendToAuthorization([FromUri] string transactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.SendToAuthorization(transactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{transactionUID:guid}")]
    public SingleObjectModel UpdateTransaction([FromUri] string transactionUID,
                                               [FromBody] BudgetTransactionFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.UpdateTransaction(transactionUID, fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }

    #endregion Web Apis

  }  // class BudgetTransactionsController



  public class RejectFields {

    public string Message {
      get; set;
    } = "No se indicó el motivo.";

  }  // class RejectFields


  public class BulkOperationCommand {

    public string[] Items {
      get; set;
    }

  }  // class BulkOperationCommand

}  // namespace Empiria.Budgeting.Transactions.WebApi
