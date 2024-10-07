/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project Types                                Component : Web Api                               *
*  Assembly : Empiria.Projects.WebApi.dll                  Pattern   : Query Controller                      *
*  Type     : ProjectTypesQueryController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive project types.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Projects.UseCases;
using Empiria.Projects.Adapters;

namespace Empiria.Projects.WebApi {

  /// <summary>Query web API used to retrive project types.</summary>
  public class ProjectTypesQueryController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/budgeting/project-types")]
    public CollectionModel GetProjectTypes() {

      using (var usecases = ProjectTypesUseCases.UseCaseInteractor()) {
        FixedList<ProjectTypeDto> list = usecases.ProjectTypesList();

        return new CollectionModel(base.Request, list);
      }
    }

    #region Command web apis

    [HttpPost]
    [Route("v2/budgeting/project-types")]
    public SingleObjectModel AddProjectType([FromBody] ProjectTypeFields fields) {

      base.RequireBody(fields);

      using (var usecases = ProjectTypesUseCases.UseCaseInteractor()) {
        ProjectTypeDto projectType = usecases.AddProjectType(fields);

        return new SingleObjectModel(base.Request, projectType);
      }
    }

    #endregion Command web apis

    #endregion Web Apis

  }  // class ProjectTypesQueryController

}  // namespace Empiria.Projects.WebApi
