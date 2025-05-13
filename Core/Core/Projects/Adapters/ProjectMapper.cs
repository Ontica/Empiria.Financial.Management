/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
*  Type     : ProjectMapper                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial projects.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects.Adapters {

  /// <summary> Mapping methods for financial projects.</summary>
  static public class ProjectMapper {

    static public FixedList<ProjectDto> Map(FixedList<Project> project) {
      return project.Select(x => Map(x)).ToFixedList();
    }

    static public ProjectDto Map(Project project) {
      return new ProjectDto {
        UID = project.UID,
        StandarAccount = project.StandarAccountId,
        CategoryId = project.CategoryId,
        PrjNo = project.PrjNo,
        Name = project.Name,  
        OrganizationUnit = project.OrganizationUnit.MapToNamedEntity(),
        Status = project.Status,
      };
    }

  }  // class BudgetAccountSegmentMapper

}  // namespace Empiria.Budgeting.Adapters
