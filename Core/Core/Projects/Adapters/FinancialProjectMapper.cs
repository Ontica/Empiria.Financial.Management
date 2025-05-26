/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
*  Type     : FinancialProjectMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial projects.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary> Mapping methods for financial projects.</summary>
  static public class FinancialProjectMapper {

    #region Mappers

    static public FixedList<FinancialProjectDescriptor> Map(FixedList<FinancialProject> project) {
      return project.Select(x => MapToDescriptor(x))
                    .ToFixedList();
    }


    static public FinancialProjectDto Map(FinancialProject project) {
      return new FinancialProjectDto {
        UID = project.UID,
        Category = project.Category.MapToNamedEntity(),
        Program = project.Program.MapToNamedEntity(),
        Subprogram = project.Subprogram.MapToNamedEntity(),
        ProjectNo = project.ProjectNo,
        Name = project.Name,
        Party = project.Party.MapToNamedEntity(),
        Status = project.Status,
      };
    }

    #endregion Mappers

    #region Helpers

    static private FinancialProjectDescriptor MapToDescriptor(FinancialProject project) {
      return new FinancialProjectDescriptor {
        UID = project.UID,
        CategoryName = project.Category.Name,
        ProgramName = project.Program.Name,
        SubprogramName = project.Subprogram.Name,
        ProjectNo = project.ProjectNo,
        Name = project.Name,
        PartyName = project.Party.Name,
        StatusName = project.Status.GetName(),
      };
    }

    #endregion Helpers

  }  // class FinancialProjectMapper

}  // namespace Empiria.Financial.Projects.Adapters
