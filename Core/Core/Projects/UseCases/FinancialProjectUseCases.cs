/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialProjectUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial projects.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.Services;

using Empiria.Financial.Projects.Adapters;
using Empiria.Financial.Projects.Data;

namespace Empiria.Financial.Projects.UseCases {

  /// <summary>Provides use cases for update and retrieve financial projects.</summary>
  public class FinancialProjectUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialProjectUseCases() {
      // no-op
    }

    static public FinancialProjectUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialProjectUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FinancialProjectHolderDto AutorizeProject(string financialProjectUID) {
      Assertion.Require(financialProjectUID, nameof(financialProjectUID));

      var project = FinancialProject.Parse(financialProjectUID);

      project.Authorize();

      project.Save();

      return FinancialProjectMapper.Map(project);
    }


    public FinancialProjectHolderDto CreateProject(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var category = FinancialProjectCategory.Parse(fields.CategoryUID);
      var orgUnit = OrganizationalUnit.Parse(fields.BaseOrgUnitUID);

      var subprograms = FinancialProject.GetClassificationList(FinancialProjectClassificator.Subprograma);

      var subprogram = subprograms.Find(x => x.UID == fields.SubprogramUID);

      var project = new FinancialProject(category, orgUnit, subprogram, fields.Name);

      project.Update(fields);

      project.Save();

      return FinancialProjectMapper.Map(project);
    }


    public FinancialProjectHolderDto DeleteProject(string financialProjectUID) {
      Assertion.Require(financialProjectUID, nameof(financialProjectUID));

      var project = FinancialProject.Parse(financialProjectUID);

      project.Delete();

      project.Save();

      return FinancialProjectMapper.Map(project);
    }


    public FinancialProjectHolderDto GetProject(string financialProjectUID) {
      Assertion.Require(financialProjectUID, nameof(financialProjectUID));

      var project = FinancialProject.Parse(financialProjectUID);

      return FinancialProjectMapper.Map(project);
    }


    public FixedList<NamedEntityDto> GetProjectsCategories() {
      return FinancialProjectCategory.GetList()
                                     .MapToNamedEntityList();

    }


    public FixedList<NamedEntityDto> GetProjectsPrograms() {
      var classificator = FinancialProjectClassificator.Programa;
      FixedList<INamedEntity> programs = FinancialProject.GetClassificationList(classificator);

      return programs.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetProjectsSubprograms() {
      var classificator = FinancialProjectClassificator.Subprograma;

      FixedList<INamedEntity> programs = FinancialProject.GetClassificationList(classificator);

      return programs.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialProject> projects = FinancialProjectDataService.SearchProjects(keywords);

      return projects.MapToNamedEntityList();
    }


    public FixedList<FinancialProjectDescriptor> SearchProjects(FinancialProjectQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialProject> projects = FinancialProjectDataService.SearchProjects(filter, sort);

      projects = query.ApplyRemainingFilters(projects);

      return FinancialProjectMapper.Map(projects);
    }


    public FinancialProjectHolderDto UpdateProject(string UID, FinancialProjectFields fields) {

      Assertion.Require(UID, nameof(UID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var project = FinancialProject.Parse(UID);

      project.Update(fields);

      project.Save();

      return FinancialProjectMapper.Map(project);
    }

    #endregion Use cases

  }  // class FinancialProjectUseCases

}  // namespace Empiria.Financial.Projects.UseCases
