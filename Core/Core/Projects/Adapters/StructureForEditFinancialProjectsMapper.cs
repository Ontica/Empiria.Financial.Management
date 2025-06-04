/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Mapper                                *
*  Type     : StructureForEditFinancialProjectsMapper      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Maps structured data for use in financial projects edition.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Maps structured data for use in financial projects edition.</summary>
  static internal class StructureForEditFinancialProjectsMapper {

    static internal FixedList<StructureForEditFinancialProjects> Map(FixedList<OrganizationalUnit> list) {
      return list.Select(x => Map(x))
                 .ToFixedList();
    }

    #region Helpers

    private static StructureForEditFinancialProjects Map(OrganizationalUnit orgUnit) {
      return new StructureForEditFinancialProjects {
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

  }  // class StructureForEditFinancialProjectsMapper

}  // namespace Empiria.Financial.Projects.Adapters

