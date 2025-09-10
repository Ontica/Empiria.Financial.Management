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

    #region Credit System Web Apis

    [HttpGet]
    [Route("v2/financial-accounts/external-systems/credit/{accountNo}/search")]
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

    #endregion Credit System Web Apis

  }  // class ExternalAccountsController

}  // namespace Empiria.Financial.Projects.WebApi
