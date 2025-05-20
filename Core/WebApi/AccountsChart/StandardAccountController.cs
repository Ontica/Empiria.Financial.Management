/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : StandardAccountController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update standard accounts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;
using Empiria.WebApi;

using Empiria.Financial.Accounts.UseCases;

namespace Empiria.Financial.Accounts.WebApi {

  /// <summary>Web API used to retrive and update standard accounts.</summary>
  public class StandardAccountController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/standard-accounts/{categoryId}")]
    public CollectionModel GetStandardAccountByCategory([FromUri] int categoryId) {

      using (var usecases = StandardAccountUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> stdAccounts = usecases.GetStandardAccountByCategory(categoryId);

        return new CollectionModel(base.Request, stdAccounts);
      }
    }

    #endregion Query web apis

    #region Web Apis
    #endregion Web Apis

  }  // class StandardAccountController

}  // namespace Empiria.Financial.Projects.WebApi
