/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Adapters Layer                        *
*  Assembly : Empiria.CashFlow.Core.dll                    Pattern   : Output DTO                            *
*  Type     : StructureForEditCashFlowProjections          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO used to return structured data for cash flow projections for edition.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Output DTO used to return structured data for cash flow projections edition.</summary>
  public class StructureForEditCashFlowProjections {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Plans {
      get; internal set;
    }

    public FixedList<ProjectionTypeForEditionDto> ProjectionTypes {
      get; internal set;
    }

  }  // class StructureForEditCashFlowProjections



  /// <summary>Output DTO used to return project categories for cash flow projections edition.</summary>
  public class ProjectionTypeForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectTypeForEditionDto> ProjectTypes {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Sources {
      get; internal set;
    }

  }  // class ProjectionTypeForEditionDto



  /// <summary>Output DTO used to return project types for cash flow projections edition.</summary>
  public class ProjectTypeForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<ProjectionProjectForEdition> Projects {
      get; internal set;
    }

  }  // class ProjectTypeForEditionDto



  /// <summary>Output DTO used to return financial projects for cash flow projections edition.</summary>
  public class ProjectionProjectForEdition {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Accounts {
      get; internal set;
    }

  }  // ProjectionProjectForEdition

}  // namespace Empiria.CashFlow.Projections.Adapters
