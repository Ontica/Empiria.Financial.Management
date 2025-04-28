/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web Api Controller                    *
*  Type     : BudgetTransactionsController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and edit budget transactions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.StateEnums;

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
    [Route("v2/budgeting/transactions")]
    public SingleObjectModel CreateTransaction([FromBody] BudgetTransactionFields fields) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        BudgetTransactionHolderDto transaction = usecases.CreateTransaction(fields);

        return new SingleObjectModel(base.Request, transaction);
      }
    }


    [HttpDelete]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}")]
    public NoDataModel DeleteTransaction([FromUri] string budgetTransactionUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        _ = usecases.DeleteTransaction(budgetTransactionUID);

        return new NoDataModel(base.Request);
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
    public SingleObjectModel SearchTransactions([FromBody] BudgetTransactionsQuery query) {

      using (var usecases = BudgetTransactionUseCases.UseCaseInteractor()) {
        FixedList<BudgetTransactionDescriptorDto> transactions = usecases.SearchTransactions(query);

        return new SingleObjectModel(base.Request, transactions);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/{budgetTransactionUID:guid}/sendToAuthorization")]
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

}  // namespace Empiria.Budgeting.Transactions.WebApi
