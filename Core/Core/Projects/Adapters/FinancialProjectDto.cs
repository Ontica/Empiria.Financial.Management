/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialProjectDto                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for financial projects data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.StateEnums;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Output holder DTO used for a cash flow projection.</summary>
  public class FinancialProjectHolderDto {

    public FinancialProjectDto FinancialProject {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class FinancialProjectHolderDto



  /// <summary>Output DTO for financial projects data.</summary>
  public class FinancialProjectDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Category {
      get; internal set;
    }

    public NamedEntityDto Program {
      get; internal set;
    }

    public NamedEntityDto Subprogram {
      get; internal set;
    }

    public NamedEntityDto BaseOrgUnit {
      get; internal set;
    }

    public string ProjectNo {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public EntityStatus Status {
      get; internal set;
    }

  }  // class FinancialProjectDto



  /// <summary>Output DTO for financial projects data for use in lists.</summary>
  public class FinancialProjectDescriptor {

    public string UID {
      get; internal set;
    }

    public string CategoryName {
      get; internal set;
    }

    public string ProgramName {
      get; internal set;
    }

    public string SubprogramName {
      get; internal set;
    }

    public string BaseOrgUnitName {
      get; internal set;
    }

    public string ProjectNo {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class FinancialProjectDescriptor

}  // namespace Empiria.Financial.Projects.Adapters
