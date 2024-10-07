/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Adapters Layer                          *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Mapping class                           *
*  Type     : ProjectMapper                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps Project instances to data transfer objects.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Projects.Adapters {

  /// <summary>Maps Project instances to data transfer objects.</summary>
  static internal class ProjectMapper {

    static internal FixedList<ProjectDto> Map(FixedList<Project> projects) {
      return projects.Select(x => Map(x)).ToFixedList();
    }

    
    static internal ProjectDto Map(Project project) {
      return new ProjectDto {
        UID = project.UID,
        Name = project.Name,
        Description = project.Description,
        Code = project.Code,

      };
    }

  }  // class ProjectMapper

}  // namespace Empiria.Projects.Adapters
