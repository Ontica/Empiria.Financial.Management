/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowAccountsController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update cash flow related financial accounts.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.UseCases;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Web API used to retrieve and update cash flow related financial accounts.</summary>
  public class CashFlowAccountsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cash-flow/accounts")]
    public CollectionModel GetFinancialAccounts([FromUri] string keywords = "") {

      using (var usecases = FinancialAccountUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> accounts = usecases.SearchAccounts(keywords);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Query web apis

  }  // class CashFlowAccountsController

}  // namespace Empiria.CashFlow.WebApi
