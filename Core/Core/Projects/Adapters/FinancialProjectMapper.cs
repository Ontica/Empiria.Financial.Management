/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
*  Type     : FinancialProjectMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial projects.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects.Adapters {

  /// <summary> Mapping methods for financial projects.</summary>
  static public class FinancialProjectMapper {

    static public FixedList<FinancialProjectDto> Map(FixedList<FinancialProject> project) {
      return project.Select(x => Map(x)).ToFixedList();
    }


    static public FinancialProjectDto Map(FinancialProject project) {
      return new FinancialProjectDto {
        UID = project.UID,
        StandardAccount = project.StandardAccount.MapToNamedEntity(),
        Category = project.Category.MapToNamedEntity(),
        ProjectNo = project.ProjectNo,
        Name = project.Name,
        OrganizationalUnit = project.OrganizationalUnit.MapToNamedEntity(),
        Status = project.Status,
      };
    }

  }  // class FinancialProjectMapper

}  // namespace Empiria.Financial.Projects.Adapters
