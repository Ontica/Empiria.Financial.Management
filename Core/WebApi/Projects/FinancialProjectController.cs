/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialProjectController                   License   : Please read LICENSE.txt file          *
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
  public class FinancialProjectController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/financial-projects/categories")]
    public CollectionModel GetProjectCategories() {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> projects = usecases.GetProjectCategories();

        return new CollectionModel(base.Request, projects);
      }
    }


    [HttpGet]
    [Route("v2/financial-projects/{keywords}")]
    public CollectionModel SearchProjects([FromUri] string keywords = "") {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> projects = usecases.SearchProjects(keywords);

        return new CollectionModel(base.Request, projects);
      }
    }


    [HttpPost]
    [Route("v2/financial-projects/search")]
    public CollectionModel SearchProjects([FromBody] FinancialProjectQuery query) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<FinancialProjectDto> projects = usecases.SearchProjects(query);

        return new CollectionModel(base.Request, projects);
      }
    }


    #endregion Query web apis

    #region Web Apis

    [HttpPost]
    [Route("v2/financial-projects")]
    public SingleObjectModel CreateFinancialProject([FromBody] FinancialProjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FinancialProjectDto projects = usecases.CreateProject(fields);

        return new SingleObjectModel(base.Request, projects);
      }
    }


    [HttpDelete]
    [Route("v2/financial-projects/{financialProjectUID:guid}")]
    public NoDataModel DeleteFinancialProject([FromUri] string financialProjectUID) {

      base.RequireResource(financialProjectUID, nameof(financialProjectUID));

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {

        usecases.DeleteProject(financialProjectUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpPut]
    [Route("v2/financial-projects/{financialProjectUID:guid}")]
    public SingleObjectModel UpdateFinancialProject([FromUri] string financialProjectUID,
                                               [FromBody] FinancialProjectFields fields) {

      base.RequireBody(fields);

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {

        FinancialProjectDto projects = usecases.UpdateProject(financialProjectUID,
                                                                         fields);

        return new SingleObjectModel(this.Request, projects);
      }
    }

    #endregion Web Apis

  }  // class FinancialProjectController

}  // namespace Empiria.Financial.Projects.WebApi
