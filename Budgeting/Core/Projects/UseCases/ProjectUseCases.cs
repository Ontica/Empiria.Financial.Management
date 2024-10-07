/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Use cases Layer                         *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : ProjectsUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for project segment items.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;

using Empiria.Projects.Adapters;
using Empiria.ProjectTypes;

namespace Empiria.Projects.UseCases {

  /// <summary>Use cases for project segment items.</summary>
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


    public ProjectDto AddProject(ProjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var project = new Project(fields);

      project.Save();

      return ProjectMapper.Map(project);
    }


    public FixedList<ProjectDto> ProjectList(string projectTypeUID) {
      Assertion.Require(projectTypeUID, nameof(projectTypeUID));

      var projectType = ProjectType.Parse(projectTypeUID);

      FixedList<Project> values = Project.GetList(projectType);

      return ProjectMapper.Map(values);
    }


    public FixedList<ProjectDto> ProjectList() {

      FixedList<Project> values = Project.GetList();

      return ProjectMapper.Map(values);
    }

    #endregion Use cases

  }  // class ProjectSegmentItemsUseCases

}  // namespace Empiria.Projects.UseCases
