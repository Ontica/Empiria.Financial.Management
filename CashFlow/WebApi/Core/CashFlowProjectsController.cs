/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Web Api                               *
*  Assembly : Empiria.CashFlow.WebApi.dll                  Pattern   : Web api controller                    *
*  Type     : CashFlowProjectsController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update cash flow related financial projects.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Projects.UseCases;

namespace Empiria.CashFlow.WebApi {

  /// <summary>Web API used to retrieve and update cash flow related financial projects.</summary>
  public class CashFlowProjectsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/cash-flow/projects")]
    public CollectionModel GetFinancialProjects([FromUri] string keywords = "") {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> projects = usecases.SearchProjects(keywords);

        return new CollectionModel(base.Request, projects);
      }
    }

    #endregion Query web apis

  }  // class CashFlowProjectsController

}  // namespace Empiria.CashFlow.WebApi
