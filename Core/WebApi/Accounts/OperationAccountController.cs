/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : OperationAccountController                  License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Web API used to retrive and update operation accounts belonging to financial accounts.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;
using Empiria.Financial.UseCases;

namespace Empiria.Financial.Accounts.WebApi {

  /// <summary>Web API used to retrive and update operation accounts belonging to financial accounts.</summary>
  public class OperationAccountController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/financial-accounts/{accountUID:guid}/operations")]
    public SingleObjectModel GetAccountOperations([FromUri] string accountUID) {

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        OperationAccountsStructure operations = usecases.GetOperationAccounts(accountUID);

        return new SingleObjectModel(base.Request, operations);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/{projectUID:guid}/accounts/{accountUID:guid}/operations")]
    public SingleObjectModel GetProjectAccountOperations([FromUri] string projectUID,
                                                         [FromUri] string accountUID) {

      var fields = new FinancialAccountFields {
        UID = accountUID,
        ProjectUID = projectUID,
      };

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        OperationAccountsStructure operations = usecases.GetProjectOperationAccounts(fields);

        return new SingleObjectModel(base.Request, operations);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/financial-accounts/{accountUID:guid}/operations")]
    public SingleObjectModel AddAccountOperation([FromUri] string accountUID,
                                                 [FromBody] OperationAccountFields fields) {

      fields.BaseAccountUID = accountUID;

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        OperationAccountsStructure operations = usecases.AddOperationAccount(fields);

        return new SingleObjectModel(base.Request, operations);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounts/{accountUID:guid}/operations/{operationAccountUID:guid}")]
    public SingleObjectModel RemoveAccountOperation([FromUri] string accountUID,
                                                    [FromUri] string operationAccountUID) {

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        OperationAccountsStructure operations = usecases.RemoveOperationAccount(accountUID,
                                                                                operationAccountUID);

        return new SingleObjectModel(base.Request, operations);
      }
    }

    #endregion Command web apis

  }  // class OperationAccountController

}  // namespace Empiria.Financial.Projects.WebApi
