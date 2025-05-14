/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : ProjectUseCases                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial projects.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Projects.Adapters;
using Empiria.Services;

namespace Empiria.Financial.Projects.UseCases {

  /// <summary>Provides use cases for update and retrieve financial projects.</summary>
  public class ProjectUseCases : UseCase {

    #region Constructors and parsers

    protected ProjectUseCases() {
      // no-op
    }

    static public ProjectUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ProjectUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ProjectDto CreateProject(ProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var project = new FinancialProject(fields);

      project.Save();

      return ProjectMapper.Map(project);
    }

    public ProjectDto UpdateProject(string UID, ProjectFields fields) {

      Assertion.Require(UID, nameof(UID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var project = FinancialProject.Parse(UID);

      project.Update(fields);

      project.Save();

      return ProjectMapper.Map(project);
    }

    public void DeleteProject(string UID) {
      Assertion.Require(UID, nameof(UID));
      var project = FinancialProject.Parse(UID);

      project.Delete();

      project.Save();

    }

    public FixedList<NamedEntityDto> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialProject> projects = FinancialProject.SearchProjects(keywords);

      return projects.MapToNamedEntityList();
    }


    public FixedList<ProjectDto> SearchProjects(ProjectQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialProject> projects = FinancialProject.SearchProjects(filter, sort);

      return ProjectMapper.Map(projects);
    }


    #endregion Use cases

  }  // class ProjectUseCases

}  // namespace Empiria.Financial.Projects.UseCases
