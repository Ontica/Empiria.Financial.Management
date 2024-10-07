/* Empiria Financial *****************************************************************************************
*                                                                                                             *
*  Module   : ProjectType                                 Component : Adapters Layer                          *
*  Assembly : Empiria.Projects.Core.dll                   Pattern   : Mapping class                           *
*  Type     : ProjectTypesMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                             *
*  Summary  : Maps ProjectType instances to data transfer objects.                                            *
*                                                                                                             *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.ProjectTypes;

namespace Empiria.Projects.Adapters {

  /// <summary>Maps ProjectType instances to data transfer objects.</summary>
  static internal class ProjectTypesMapper {

    #region Mappers

    static internal FixedList<ProjectTypeDto> Map(FixedList<ProjectType> projectTypes) {
      return projectTypes.Select(x => Map(x)).ToFixedList();
    }

    static internal ProjectTypeDto Map(ProjectType ProjectType) {
      return new ProjectTypeDto {

        UID = ProjectType.UID,
        Name = ProjectType.Name,
        Code = ProjectType.Code
        //Projects = ProjectMapper.Map(Projects.FindAll(x => x.ProjectType.Equals(ProjectType)))
      };
    }

    #endregion Mappers

  }  // class ProjectTypesMapper

}  // namespace Empiria.Projects.Adapters
