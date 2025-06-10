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

using Empiria.Parties;
using Empiria.WebApi;

using Empiria.Financial.Projects.UseCases;
using Empiria.Financial.Projects.Adapters;

namespace Empiria.Financial.Projects.WebApi {

  /// <summary>Web API used to retrive and update financial projects.</summary>
  public class FinancialProjectController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v1/financial-projects/organizational-units/{orgUnitUID:guid}/assignees")]
    public CollectionModel GetProjectAssignees([FromUri] string orgUnitUID) {

      FixedList<NamedEntityDto> assignees = BaseObject.GetFullList<Person>()
                                                      .MapToNamedEntityList();

      return new CollectionModel(base.Request, assignees);
    }


    [HttpGet]
    [Route("v1/financial-projects/categories")]
    public CollectionModel GetProjectCategories() {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> categories = usecases.GetProjectCategories();

        return new CollectionModel(base.Request, categories);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/programs")]
    public CollectionModel GetProjectsPrograms() {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> programs = usecases.GetProjectsPrograms();

        return new CollectionModel(base.Request, programs);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/subprograms")]
    public CollectionModel GetProjectsSubprograms() {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> stdAccounts = usecases.GetProjectsSubprograms();

        return new CollectionModel(base.Request, stdAccounts);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/organizational-units/{orgUnitUID:guid}/structured-data-for-edition")]
    public SingleObjectModel GetStructureForEditProjects([FromUri] string orgUnitUID) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        StructureForEditFinancialProjects structure = usecases.GetStructureForEditProjects(orgUnitUID);

        return new SingleObjectModel(base.Request, structure);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/search")]
    public CollectionModel SearchProjects([FromUri] string keywords = "") {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> projects = usecases.SearchProjects(keywords);

        return new CollectionModel(base.Request, projects);
      }
    }


    [HttpPost]
    [Route("v1/financial-projects/search")]
    public CollectionModel SearchProjects([FromBody] FinancialProjectQuery query) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FixedList<FinancialProjectDescriptor> projects = usecases.SearchProjects(query);

        return new CollectionModel(base.Request, projects);
      }
    }

    #endregion Query web apis

    #region Command Web Apis

    [HttpPost]
    [Route("v1/financial-projects")]
    public SingleObjectModel CreateFinancialProject([FromBody] FinancialProjectFields fields) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FinancialProjectHolderDto projects = usecases.CreateProject(fields);

        return new SingleObjectModel(base.Request, projects);
      }
    }


    [HttpDelete]
    [Route("v1/financial-projects/{financialProjectUID:guid}")]
    public NoDataModel DeleteFinancialProject([FromUri] string financialProjectUID) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {

        _ = usecases.DeleteProject(financialProjectUID);

        return new NoDataModel(this.Request);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/{financialProjectUID:guid}")]
    public SingleObjectModel GetProject([FromUri] string financialProjectUID) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FinancialProjectHolderDto project = usecases.GetProject(financialProjectUID);

        return new SingleObjectModel(base.Request, project);
      }
    }


    [HttpGet]
    [Route("v1/financial-projects/{financialProjectUID:guid}/plain")]
    public SingleObjectModel GetPlainProject([FromUri] string financialProjectUID) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {
        FinancialProjectDto project = usecases.GetPlainProject(financialProjectUID);

        return new SingleObjectModel(base.Request, project);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/financial-projects/{financialProjectUID:guid}")]
    public SingleObjectModel UpdateFinancialProject([FromUri] string financialProjectUID,
                                                    [FromBody] FinancialProjectFields fields) {

      using (var usecases = FinancialProjectUseCases.UseCaseInteractor()) {

        FinancialProjectHolderDto project = usecases.UpdateProject(financialProjectUID, fields);

        return new SingleObjectModel(this.Request, project);
      }
    }

    #endregion Command Web Apis

  }  // class FinancialProjectController

}  // namespace Empiria.Financial.Projects.WebApi
