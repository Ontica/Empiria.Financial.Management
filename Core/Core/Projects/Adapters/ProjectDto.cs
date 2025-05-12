/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : ProjectDto                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO with financial projects data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;
using System;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Output DTO with financial projects data.</summary>
  public class ProjectDto {

    public string UID {
      get; set;
    }


    public int StandarAccount {
      get; set;
    }


    public int CategoryId {
      get; set;
    }


    public string PrjNo {
      get; set;
    }


    public string Name {
      get; set;
    }


    public string OrganizationUnit {
      get; set;
    }

    
    public DateTime StartDate {
      get; set;
    }


    public DateTime EndDate {
      get; set;
    }


    public string PostedBy {
      get; set;
    }


    public DateTime PostingTime {
      get; set;
    }


    public EntityStatus Status {
      get; set;
    }


  }  // class ProjectDto

}  // namespace Empiria.Financial.Projects.Adapters
