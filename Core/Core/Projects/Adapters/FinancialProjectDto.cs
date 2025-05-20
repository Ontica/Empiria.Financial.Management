/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialProjectDto                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for financial projects data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Output DTO for financial projects data.</summary>
  public class FinancialProjectDto {

    public string UID {
      get; set;
    }


    public NamedEntityDto StandardAccount {
      get; set;
    }


    public NamedEntityDto Category {
      get; set;
    }


    public string ProjectNo {
      get; set;
    }


    public string Name {
      get; set;
    }


    public NamedEntityDto OrganizationalUnit {
      get; set;
    }


    public EntityStatus Status {
      get; set;
    }

  }  // class FinancialProjectDto

}  // namespace Empiria.Financial.Projects.Adapters
