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
        FixedList<FinancialAccountDto> accounts = usecases.SearchAccount(query);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Query web apis

    #region Web Apis

    [HttpPost]
    [Route("v2/financial-accounts")]
    public SingleObjectModel CreateFinancialAccount([FromBody] FinancialAccountFields fields) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {
        FinancialAccountDto createAccount = usecases.CreateAccount(fields);

        return new SingleObjectModel(base.Request, createAccount);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounts/{financialAccountUID:guid}")]
    public NoDataModel DeleteAccount([FromUri] string financialAccountUID) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {

        usecases.DeleteAccount(financialAccountUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v2/financial-accounts/{financialAccountUID:guid}")]
    public SingleObjectModel UpdateAccount([FromUri] string financialAccountUID,
                                           [FromBody] FinancialAccountFields fields) {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {

        FinancialAccountDto paymentOrder = usecases.UpdateAccount(financialAccountUID, fields);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }

    #endregion Web Apis

  }  // class FinancialAccountController

}  // namespace Empiria.Financial.Projects.WebApi
