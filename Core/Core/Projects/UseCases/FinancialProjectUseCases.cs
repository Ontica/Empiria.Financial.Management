/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialProjectUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial projects.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Projects.Adapters;

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

    public FinancialProjectDto CreateProject(FinancialProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var project = new FinancialProject(fields);

      project.Save();

      return FinancialProjectMapper.Map(project);
    }


    public void DeleteProject(string UID) {
      Assertion.Require(UID, nameof(UID));
      var project = FinancialProject.Parse(UID);

      project.Delete();

      project.Save();
    }


    public FinancialProjectDto GetProject(string UID) {
      Assertion.Require(UID, nameof(UID));
      var project = FinancialProject.Parse(UID);

      return FinancialProjectMapper.Map(project);
    }


    public FixedList<NamedEntityDto> GetProjectCategories() {
      return FinancialProjectCategory.GetList().MapToNamedEntityList();

    }


    public FinancialProjectCategory GetProjectCategory(string UID) {
      return FinancialProjectCategory.Parse(UID);
    }


    public FixedList<NamedEntityDto> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialProject> projects = FinancialProject.SearchProjects(keywords);

      return projects.MapToNamedEntityList();
    }


    public FixedList<FinancialProjectDto> SearchProjects(FinancialProjectQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialProject> projects = FinancialProject.SearchProjects(filter, sort);

      return FinancialProjectMapper.Map(projects);
    }


    public FinancialProjectDto UpdateProject(string UID, FinancialProjectFields fields) {

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
