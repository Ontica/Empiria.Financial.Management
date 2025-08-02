/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Structurer mapper                     *
*  Type     : StructureForEditFinancialProjectsMapper      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Maps structured data for use in financial projects edition.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Maps structured data for use in financial projects edition.</summary>
  static internal class StructureForEditFinancialProjectsMapper {

    static private FixedList<FinancialProgram> _allSubPrograms =
                                                  FinancialProgram.GetList(FinancialProgramType.Subprograma);

    static private FixedList<FinancialProjectCategory> _allProjectTypes = FinancialProjectCategory.GetList();

    static internal StructureForEditFinancialProjects Map(OrganizationalUnit orgUnit) {
      var structurer = new Structurer(orgUnit);

      return structurer.MapPrograms();
    }

    private class Structurer {

      private readonly OrganizationalUnit _orgUnit;
      private readonly FixedList<FinancialProgram> _programs;

      internal Structurer(OrganizationalUnit orgUnit) {
        _orgUnit = orgUnit;

        var orgUnitPrograms = _orgUnit.ExtendedData.GetList<string>("financialPrograms");

        _programs = _allSubPrograms.FindAll(x => orgUnitPrograms.Contains(x.Parent.ProgramNo))
                                    .SelectDistinct(x => x.Parent);
      }


      private ProjectProgramForEditionDto MapProgram(FinancialProgram program) {
        return new ProjectProgramForEditionDto {
          UID = program.UID,
          Name = $"({program.ProgramNo}) {program.Name}",
          Subprograms = MapSubprograms(program.Children)
        };
      }


      internal StructureForEditFinancialProjects MapPrograms() {
        return new StructureForEditFinancialProjects {
          UID = _orgUnit.UID,
          Name = _orgUnit.Name,
          Programs = _programs.Select(x => MapProgram(x))
                              .ToFixedList()
        };
      }


      private FixedList<NamedEntityDto> MapProjectTypes(FinancialProgram subprogram) {
        var projectTypes = _allProjectTypes.FindAll(x => subprogram.ProgramNo.EndsWith($".{x.SubprogramCode}"));

        return projectTypes.Select(x => x.MapToNamedEntity())
                           .ToFixedList();
      }


      private ProjectSubprogramForEditionDto MapSubprogram(FinancialProgram subprogram) {
        return new ProjectSubprogramForEditionDto {
           UID = subprogram.UID,
           Name = $"({subprogram.ProgramNo}) {subprogram.Name}",
           ProjectTypes = MapProjectTypes(subprogram)
        };
      }


      private FixedList<ProjectSubprogramForEditionDto> MapSubprograms(FixedList<FinancialProgram> subprograms) {
        return subprograms.Select(x => MapSubprogram(x))
                          .ToFixedList();
      }

    }  // class Structurer

  }  // class StructureForEditFinancialProjectsMapper

}  // namespace Empiria.Financial.Projects.Adapters
