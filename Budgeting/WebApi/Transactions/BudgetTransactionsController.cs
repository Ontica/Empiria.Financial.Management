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

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;

namespace Empiria.Budgeting.Transactions.WebApi {

  /// <summary>Web API used to retrieve and edit budget transactions.</summary>
  public class BudgetTransactionsController : WebApiController {

    #region Web Apis

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
    [Route("v2/budgeting/transactions/parties")]
    public CollectionModel SearchTransactionsParties([FromBody] BudgetPartiesQuery query) {

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

    #endregion Web Apis

  }  // class BudgetTransactionsController

}  // namespace Empiria.Budgeting.Transactions.WebApi
