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

        FinancialAccountOperationsDto operations = usecases.GetAccountOperations(accountUID);

        return new SingleObjectModel(base.Request, operations);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/{financialProjectUID:guid}/accounts/{accountUID:guid}/operations")]
    public SingleObjectModel GetProjectAccountOperations([FromUri] string financialProjectUID,
                                                         [FromUri] string accountUID) {

      var fields = new FinancialAccountFields {
        UID = accountUID,
        ProjectUID = financialProjectUID,
      };

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        FinancialAccountOperationsDto operations = usecases.GetFinancialProjectAccountOperations(fields);

        return new SingleObjectModel(base.Request, operations);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/financial-accounts/{accountUID:guid}/operations/{stdAccountUID:guid}")]
    public SingleObjectModel AddAccountOperation([FromUri] string accountUID,
                                                 [FromUri] string stdAccountUID) {

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        FinancialAccountOperationsDto operations = usecases.AddAccountOperation(accountUID, stdAccountUID);

        return new SingleObjectModel(base.Request, operations);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounts/{accountUID:guid}/operations/{operationAccountUID:guid}")]
    public SingleObjectModel RemoveAccountOperation([FromUri] string accountUID,
                                                    [FromUri] string operationAccountUID) {

      using (var usecases = OperationAccountUseCases.UseCaseInteractor()) {

        FinancialAccountOperationsDto operations = usecases.RemoveAccountOperation(accountUID,
                                                                                   operationAccountUID);

        return new SingleObjectModel(base.Request, operations);
      }
    }

    #endregion Command web apis

  }  // class OperationAccountController

}  // namespace Empiria.Financial.Projects.WebApi
