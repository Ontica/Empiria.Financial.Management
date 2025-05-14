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
        
    #region Query web apis

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

    #endregion Query web apis

    #region Web Apis

    [HttpPost]
    [Route("v2/financial-projects")]
    public SingleObjectModel CreateFinancialProject([FromBody] ProjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {
        ProjectDto paymentOrder = usecases.CreateProject(fields);

        return new SingleObjectModel(base.Request, paymentOrder);
      }
    }


    [HttpDelete]
    [Route("v2/financial-projects/{financialProjectUID:guid}")]
    public NoDataModel DeletePaymentOrder([FromUri] string financialProjectUID) {

      base.RequireResource(financialProjectUID, nameof(financialProjectUID));

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {

        usecases.DeleteProject(financialProjectUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v2/financial-projects/{financialProjectUID:guid}")]
    public SingleObjectModel UpdatePaymentOrder([FromUri] string financialProjectUID,
                                               [FromBody] ProjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = ProjectUseCases.UseCaseInteractor()) {

        ProjectDto paymentOrder = usecases.UpdateProject(financialProjectUID,
                                                                         fields);

        return new SingleObjectModel(this.Request, paymentOrder);
      }
    }


    #endregion Web Apis

  }  // class ProjectController

}  // namespace Empiria.Financial.Projects.WebApi
