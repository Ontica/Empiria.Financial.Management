/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : ExternalAccountsController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve accounts from external systems and update them as financial accounts. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;
using Empiria.Financial.UseCases;

namespace Empiria.Financial.Accounts.WebApi {

  /// <summary>Web API used to retrieve accounts from external systems
  /// and update them as financial accounts.</summary>
  public class ExternalAccountsController : WebApiController {

    #region Credit system web apis

    [HttpPost]
    [Route("v2/financial-accounts/external-systems/credit/{accountNo}/create")]
    public SingleObjectModel CreateAccountFromCreditSystem([FromUri] string accountNo,
                                                           [FromBody] ExternalAccountFields fields) {

      using (var usecases = ExternalAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDto accountDto = usecases.CreateAccountFromCreditSystem(accountNo,
                                                                                fields.ProjectUID);

        return new SingleObjectModel(this.Request, accountDto);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounts/external-systems/credit/{accountNo}")]
    public SingleObjectModel GetAccountFromCreditSystem([FromUri] string accountNo) {

      using (var usecases = ExternalAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDto accountDto = usecases.TryGetAccountFromCreditSystem(accountNo);

        if (accountDto == null) {
          throw new ResourceNotFoundException("ExternalCreditsSystem.CreditAccountNo.NotFound",
              $"No se encontró la cuenta '{accountNo}' en el sistema de créditos.");
        }

        return new SingleObjectModel(this.Request, accountDto);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/financial-accounts/external-systems/credit/{accountUID:guid}/refresh")]
    public SingleObjectModel RefreshAccountFromCreditSystem([FromUri] string accountUID) {

      using (var usecases = ExternalAccountsUseCases.UseCaseInteractor()) {

        FinancialAccountDto accountDto = usecases.RefreshAccountFromCreditSystem(accountUID);

        return new SingleObjectModel(this.Request, accountDto);
      }
    }

    #endregion Credit system web apis

  }  // class ExternalAccountsController



  public class ExternalAccountFields {

    public string ProjectUID {
      get; set;
    }

  }  // class ExternalAccountFields

}  // namespace Empiria.Financial.Accounts.WebApi
