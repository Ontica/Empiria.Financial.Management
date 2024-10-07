/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project Types                                Component : Web Api                               *
*  Assembly : Empiria.Propjects.WebApi.dll                 Pattern   : Query Controller                      *
*  Type     : ProjectTypesQueryController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive propject types.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Projects.UseCases;
using Empiria.Projects.Adapters;

namespace Empiria.Projects.WebApi {

  /// <summary>Query web API used to retrive project types.</summary>
  public class ProjectQueryController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/project-types/{projectTypeUID:guid}/projects")]
    public CollectionModel GetProject([FromUri] string projectTypeUID) {

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {
        FixedList<ProjectDto> list = usecases.ProjectList(projectTypeUID);

        return new CollectionModel(base.Request, list);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/projects")]
    public CollectionModel GetProject() {

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {
        FixedList<ProjectDto> list = usecases.ProjectList();

        return new CollectionModel(base.Request, list);
      }
    }

    #endregion Web Apis

  }  // class ProjectTypesQueryController

}  // namespace Empiria.Projects.WebApi
