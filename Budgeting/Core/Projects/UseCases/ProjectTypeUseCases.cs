/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Project.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : ProjectTypesUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : use cases for project type management.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Projects.Adapters;
using Empiria.ProjectTypes;

namespace Empiria.Projects.UseCases {

  /// <summary>Use cases for project management.</summary>
  public class ProjectTypesUseCases : UseCase {

    #region Constructors and parsers

    protected ProjectTypesUseCases() {
      // no-opcontra
    }

    static public ProjectTypesUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ProjectTypesUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ProjectTypeDto AddProjectType(ProjectTypeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projectType = new ProjectType(fields);

      projectType.Save();

      return ProjectTypesMapper.Map(projectType);
    }


    public ProjectTypeDto UpdateProjectType(string projectTypeUID, ProjectTypeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var projectType = ProjectType.Parse(projectTypeUID);

      projectType.Update(fields);

      projectType.Save();

      return ProjectTypesMapper.Map(projectType);
    }

    public ProjectTypeDto DeleteProjectType(string projectTypeUID) {
      Assertion.Require(projectTypeUID, nameof(projectTypeUID));

      var projectType = ProjectType.Parse(projectTypeUID);

      projectType.Delete();

      projectType.Save();

      return ProjectTypesMapper.Map(projectType);
    }

    public FixedList<ProjectTypeDto> ProjectTypesList() {
      FixedList<ProjectType> projectTypes = ProjectType.GetList();

      return ProjectTypesMapper.Map(projectTypes);
    }


    public FixedList<ProjectTypeDto> ProjectTypeListByUID(string projectTypeUID) {
      Assertion.Require(projectTypeUID, nameof(projectTypeUID));

      var projectType = ProjectType.Parse(projectTypeUID);

      FixedList<ProjectType> values = ProjectType.GetList();

      return ProjectTypesMapper.Map(values);
    }


    #endregion Use cases

  }  // class ProjectTypesUseCases
}  // namespace Empiria.Projects.UseCases
