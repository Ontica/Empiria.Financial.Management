/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Mapper                                *
*  Type     : FinancialProjectOrgUnitsForEditionMapper     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Maps organizational units for use in financial projects edition.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Maps organizational units for use in financial projects edition.</summary>
  static internal class FinancialProjectOrgUnitsForEditionMapper {

    static internal FixedList<FinancialProjectOrgUnitsForEditionDto> Map(FixedList<OrganizationalUnit> list) {
      return list.Select(x => Map(x))
                 .ToFixedList();
    }

    #region Helpers

    private static FinancialProjectOrgUnitsForEditionDto Map(OrganizationalUnit orgUnit) {
      return new FinancialProjectOrgUnitsForEditionDto {
        UID = orgUnit.UID,
        Name = orgUnit.Name,
        Categories = Map(FinancialProjectCategory.GetList())
      };
    }


    static private FixedList<ProjectCategoryForEditionDto> Map(FixedList<FinancialProjectCategory> categories) {
      return categories.Select(x => MapCategory(x))
                       .ToFixedList();
    }

    private static ProjectCategoryForEditionDto MapCategory(FinancialProjectCategory category) {
      return new ProjectCategoryForEditionDto {
        UID = category.UID,
        Name = category.Name,
        Programs = MapPrograms(FinancialProgram.GetList(category))
      };
    }

    static private FixedList<ProjectProgramForEditionDto> MapPrograms(FixedList<FinancialProgram> programs) {
      return programs.Select(x => MapProgram(x))
                     .ToFixedList();
    }

    private static ProjectProgramForEditionDto MapProgram(FinancialProgram program) {
      return new ProjectProgramForEditionDto {
        UID = program.UID,
        Name = program.Name,
        Subprograms = program.Children.MapToNamedEntityList()
      };
    }

    #endregion Helpers

  }  // class FinancialProjectOrgUnitsForEditionMapper

}  // namespace Empiria.Financial.Projects.Adapters

