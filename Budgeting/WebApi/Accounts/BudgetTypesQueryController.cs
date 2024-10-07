/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Query Controller                      *
*  Type     : BudgetTypesQueryController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive budget types.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.UseCases;

namespace Empiria.Budgeting.WebApi {

  /// <summary>Query web API used to retrive budget types.</summary>
  public class BudgetTypesQueryController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/budget-types")]
    public CollectionModel GetBudgetTypes() {

      using (var usecases = BudgetTypesUseCases.UseCaseInteractor()) {
        FixedList<BudgetTypeDto> list = usecases.BudgetTypesList();

        return new CollectionModel(base.Request, list);
      }
    }

    #endregion Web Apis

  }  // class BudgetTypesQueryController

}  // namespace Empiria.Budgeting.WebApi
