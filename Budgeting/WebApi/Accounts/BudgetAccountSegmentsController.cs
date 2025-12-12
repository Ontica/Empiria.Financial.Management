/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                              Component : Web Api                               *
*  Assembly : Empiria.Budgeting.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : BudgetAccountSegmentsController              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update budget account segments.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.UseCases;

namespace Empiria.Budgeting.WebApi {

  /// <summary>Web API used to retrieve and update budget account segments.</summary>
  public class BudgetAccountSegmentsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/budget-segment-items/by-type/{segmentTypeUID}")]
    public CollectionModel GetBudgetAccountSegmentsByType([FromUri] string segmentTypeUID,
                                                          [FromUri] string keywords = "") {

      using (var usecases = BudgetAccountSegmentUseCases.UseCaseInteractor()) {
        FixedList<BudgetAccountSegmentDto> segments = usecases.GetBudgetAccountSegmentsByType(segmentTypeUID,
                                                                                              keywords);

        return new CollectionModel(base.Request, segments);
      }
    }

    #endregion Web Apis

  }  // class BudgetAccountSegmentsController

}  // namespace Empiria.Budgeting.WebApi
