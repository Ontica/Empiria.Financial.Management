/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : StructureForEditFinancialProjects            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO used to return structured data for financial projects edition.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Output DTO used to return structured data for financial projects edition.</summary>
  public class StructureForEditFinancialProjects {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectProgramForEditionDto> Programs {
      get; internal set;
    }

  }  // class StructureForEditFinancialProjects


  /// <summary>Output DTO used to return project programs for financial projects edition.</summary>
  public class ProjectProgramForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectSubprogramForEditionDto> Subprograms {
      get; internal set;
    }

  }  // class ProjectProgramForEditionDto



  /// <summary>Output DTO used to return project subprograms with its categories.</summary>
  public class ProjectSubprogramForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> ProjectTypes {
      get; internal set;
    }

  }  // class ProjectSubprogramForEditionDto

}  // namespace Empiria.Financial.Projects.Adapters
