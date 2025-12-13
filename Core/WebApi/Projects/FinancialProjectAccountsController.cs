/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialProjectAccountsController           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update financial accounts for financial projects.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;
using Empiria.Financial.Projects.UseCases;

namespace Empiria.Financial.Projects.WebApi {

  /// <summary>Web API used to retrieve and update financial accounts for financial projects.</summary>
  public class FinancialProjectAccountsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/financial-projects/{financialProjectUID:guid}/accounts/{accountUID:guid}")]
    public SingleObjectModel GetAccount([FromUri] string financialProjectUID,
                                        [FromUri] string accountUID) {

      var fields = new FinancialAccountFields {
        UID = accountUID,
        ProjectUID = financialProjectUID,
      };

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDto account = usecases.GetAccount(fields);

        return new SingleObjectModel(base.Request, account);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/financial-accounts-types")]
    public CollectionModel GetFinancialAccountsTypes() {

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> accountTypes = usecases.GetFinancialProjectsAccountTypes();

        return new CollectionModel(base.Request, accountTypes);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/{financialProjectUID:guid}/standard-accounts")]
    public CollectionModel GetStandardAccounts([FromUri] string financialProjectUID) {

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> stdAccounts = usecases.GetStandardAccounts(financialProjectUID);

        return new CollectionModel(base.Request, stdAccounts);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v1/financial-projects/{financialProjectUID:guid}/accounts")]
    public SingleObjectModel AddAccount([FromUri] string financialProjectUID,
                                        [FromBody] FinancialAccountFields fields) {

      fields.ProjectUID = financialProjectUID;

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDescriptor account = usecases.AddAccount(fields);

        return new SingleObjectModel(base.Request, account);
      }
    }


    [HttpDelete]
    [Route("v1/financial-projects/{financialProjectUID:guid}/accounts/{accountUID:guid}")]
    public NoDataModel RemoveAccount([FromUri] string financialProjectUID,
                                     [FromUri] string accountUID) {

      var fields = new FinancialAccountFields {
        UID = accountUID,
        ProjectUID = financialProjectUID,
      };

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        _ = usecases.RemoveAccount(fields);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/financial-projects/{financialProjectUID:guid}/accounts/{accountUID:guid}")]
    public SingleObjectModel UpdateAccount([FromUri] string financialProjectUID,
                                           [FromUri] string accountUID,
                                           [FromBody] FinancialAccountFields fields) {

      fields.UID = accountUID;
      fields.ProjectUID = financialProjectUID;

      using (var usecases = FinancialProjectAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDescriptor account = usecases.UpdateAccount(fields);

        return new SingleObjectModel(this.Request, account);
      }
    }

    #endregion Command web apis

  }  // class FinancialProjectAccountsController

}  // namespace Empiria.Financial.Projects.WebApi
