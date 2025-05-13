/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : ProjectController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update financial projects.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Projects.UseCases;
using Empiria.Financial.Projects.Adapters;

namespace Empiria.Financial.Projects.WebApi {

  /// <summary>Web API used to retrive and update financial projects.</summary>
  public class ProjectController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/financial-projects/{keywords:string}")]
    public CollectionModel SearchProjects([FromUri] string keywords = "") {

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> projects = usecases.SearchProjects(keywords);

        return new CollectionModel(base.Request, projects);
      }
    }


    [HttpPost]
    [Route("v2/financial-projects/search")]
    public CollectionModel SearchProjects([FromBody] ProjectQuery query) {

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {
        FixedList<ProjectDto> projects = usecases.SearchProjects(query);

        return new CollectionModel(base.Request, projects);
      }
    }

    #endregion Web Apis

  }  // class ProjectController

}  // namespace Empiria.Financial.Projects.WebApi
