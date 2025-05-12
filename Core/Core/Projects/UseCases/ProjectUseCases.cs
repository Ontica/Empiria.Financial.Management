/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : ProjectUseCases                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial projects.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

    public FixedList<NamedEntityDto> SearchProjects(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<Project> projects = Project.SearchProjects(keywords);

      return projects.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class ProjectUseCases

}  // namespace Empiria.Financial.Projects.UseCases
