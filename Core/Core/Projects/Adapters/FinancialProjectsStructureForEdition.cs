/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialProjectsStructureForEdition         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO used to return structured data for financial projects edition.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Output DTO used to return structured data for financial projects edition.</summary>
  public class FinancialProjectsStructureForEdition {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectCategoryForEditionDto> Categories {
      get; internal set;
    }

  }  // class FinancialProjectsStructureForEdition


  /// <summary>Output DTO used to return project categories for financial projects edition.</summary>
  public class ProjectCategoryForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectProgramForEditionDto> Programs {
      get; internal set;
    }

  }  // class FinancialProjectCategoryForEditionDto



  /// <summary>Output DTO used to return project programs for financial projects edition.</summary>
  public class ProjectProgramForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Subprograms {
      get; internal set;
    }

  }  // class ProjectProgramForEditionDto

}  // namespace Empiria.Financial.Projects.Adapters
