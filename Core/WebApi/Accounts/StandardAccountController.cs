/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : StandardAccountController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update standard accounts.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Accounts.UseCases;

namespace Empiria.Financial.Accounts.WebApi {

  /// <summary>Web API used to retrieve and update standard accounts.</summary>
  public class StandardAccountController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/standard-accounts/categories/{stdAccountCategoryNamedKey}")]
    public CollectionModel GetStandardAccountByCategory([FromUri] string stdAccountCategoryNamedKey) {

      using (var usecases = StandardAccountUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> stdAccounts = usecases.GetStandardAccountsInCategory(stdAccountCategoryNamedKey);

        return new CollectionModel(base.Request, stdAccounts);
      }
    }


    [HttpGet]
    [Route("v3/standard-accounts/segments/{stdAccountCategoryNamedKey}")]
    public CollectionModel GetStandardAccountSegments([FromUri] string stdAccountCategoryNamedKey) {

      using (var usecases = StandardAccountUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> segments = usecases.GetStandardAccountsSegments(stdAccountCategoryNamedKey);

        return new CollectionModel(base.Request, segments);
      }
    }


    [HttpGet]
    [Route("v3/standard-accounts")]
    public CollectionModel GetStandardAccounts([FromUri] string keywords = "",
                                               [FromUri] int maxLevel = 2) {

      keywords = keywords ?? string.Empty;

      using (var usecases = StandardAccountUseCases.UseCaseInteractor()) {

        FixedList<NamedEntityDto> stdAccounts = usecases.GetStandardAccounts(keywords, maxLevel);

        return new CollectionModel(base.Request, stdAccounts);
      }
    }

    #endregion Query web apis

  }  // class StandardAccountController

}  // namespace Empiria.Financial.Projects.WebApi
