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

using Empiria.Financial.Reporting;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

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
    [Route("v2/budgeting/transactions/bulk-operation/export-entries")]
    public SingleObjectModel ExportBudgetTransactionEntriesToExcel([FromBody] BulkOperationCommand command) {

      FixedList<BudgetTransactionByYear> transactions =
            command.Items.Select(x => new BudgetTransactionByYear(BudgetTransaction.Parse(x)))
                         .ToFixedList()
                         .Sort((x, y) => x.Transaction.TransactionNo.CompareTo(y.Transaction.TransactionNo));

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        var result = new BulkOperationResult {
          File = reportingService.ExportTransactionEntriesToExcel(transactions),
          Message = $"Se exportaron {transactions.Count} transacciones presupuestales a Excel.",
        };

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/planning/generate")]
    public SingleObjectModel GeneratePlanningTransactions() {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

        FixedList<BudgetTransactionDescriptorDto> generated = usecases.GeneratePlanningTransactions(budget.UID);

        var result = new BulkOperationResult {
          Message = $"Se generaron {generated.Count} transacciones presupuestales para el presupuesto {budget.Name}."
        };

        return new SingleObjectModel(base.Request, result);
      }
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
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}")]
    public SingleObjectModel GetTransaction([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.GetTransaction(budgetTransactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/request-account/{requestedSegmentUID:guid}")]
    public SingleObjectModel RequestBudgetAccount([FromUri] string budgetTransactionUID,
                                                  [FromUri] string requestedSegmentUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        BudgetAccountDto requestedAccount = usecases.RequestBudgetAccount(budgetTransactionUID,
                                                                          requestedSegmentUID);

        return new SingleObjectModel(base.Request, requestedAccount);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/print")]
    public SingleObjectModel PrintTransaction([FromUri] string budgetTransactionUID) {

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        FileDto file = reportingService.ExportTransactionToPdf(transaction);

        return new SingleObjectModel(base.Request, file);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/reject")]
    public SingleObjectModel RejectTransaction([FromUri] string budgetTransactionUID,
                                               [FromBody] RejectFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.RejectTransaction(budgetTransactionUID,
                                                                            fields.Message);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/accounts/search")]
    public CollectionModel SearchTransactionAccounts([FromBody] BudgetTransactionAccountsQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<BudgetAccountDto> accounts = usecases.SearchTransactionAccounts(query);

        return new CollectionModel(base.Request, accounts);
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
    [Route("v2/budgeting/transactions/search")]
    public CollectionModel SearchTransactions([FromBody] BudgetTransactionsQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<BudgetTransactionDescriptorDto> transactions = usecases.SearchTransactions(query);

        return new CollectionModel(base.Request, transactions);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/send-to-authorization")]
    public SingleObjectModel SendToAuthorization([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.SendToAuthorization(budgetTransactionUID);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}")]
    public SingleObjectModel UpdateTransaction([FromUri] string budgetTransactionUID,
                                               [FromBody] BudgetTransactionFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.UpdateTransaction(budgetTransactionUID, fields);

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

}  // namespace Empiria.Budgeting.Transactions.WebApi
