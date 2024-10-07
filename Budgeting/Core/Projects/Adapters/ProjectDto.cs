/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Output DTO                              *
*  Type     : BudgetDto                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for Project instances.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Projects.Adapters {

    /// <summary>Output DTO for Project instances.</summary>
    public class ProjectDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Code {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

  }  // class ProjectDto

}  // namespace Empiria.Projects.Adapters
