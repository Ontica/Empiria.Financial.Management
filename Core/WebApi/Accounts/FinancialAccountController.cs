/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialAccountController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update financial accounts.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;
using Empiria.Financial.UseCases;

namespace Empiria.Financial.Accounts.WebApi {

  /// <summary>Web API used to retrive and update financial accounts.</summary>
  public class FinancialAccountController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/financial-accounts/{accountUID:guid}")]
    public SingleObjectModel GetAccount([FromUri] string accountUID) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {

        FinancialAccountHolderDto accountDto = usecases.GetAccount(accountUID);

        return new SingleObjectModel(this.Request, accountDto);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounts/{keywords}")]
    public CollectionModel SearchAccounts([FromUri] string keywords = "") {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> accounts = usecases.SearchAccounts(keywords);

        return new CollectionModel(base.Request, accounts);
      }
    }


    [HttpPost]
    [Route("v2/financial-accounts/search")]
    public CollectionModel SearchAccounts([FromBody] FinancialAccountQuery query) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {
        FixedList<FinancialAccountDescriptor> accounts = usecases.SearchAccounts(query);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/financial-accounts")]
    public SingleObjectModel CreateFinancialAccount([FromBody] FinancialAccountFields fields) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {
        FinancialAccountDto createAccount = usecases.CreateAccount(fields);

        return new SingleObjectModel(base.Request, createAccount);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounts/{accountUID:guid}")]
    public NoDataModel DeleteAccount([FromUri] string accountUID) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {

        usecases.DeleteAccount(accountUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v2/financial-accounts/{accountUID:guid}")]
    public SingleObjectModel UpdateAccount([FromUri] string accountUID,
                                           [FromBody] FinancialAccountFields fields) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {

        FinancialAccountDto paymentOrder = usecases.UpdateAccount(accountUID, fields);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }

    #endregion Command web apis

  }  // class FinancialAccountController

}  // namespace Empiria.Financial.Projects.WebApi
