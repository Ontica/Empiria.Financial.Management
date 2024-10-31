/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestones Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ContractMilestoneDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return Contract milestones information.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Parties;

namespace Empiria.Contracts.Adapters {

  /// <summary>Data transfer object used to return milestone information.</summary>
  public class ContractMilestoneDto {


    public int ID {
      get; internal set;
    }


    public string UID {
      get; internal set;
    }


    public Contract Contract {
      get; internal set;
    }


    public string MilestoneNo {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public Party Supplier {
      get; internal set;
    }


    public string PaymentExtData {
      get; internal set;
    }


    public NamedEntityDto ManagedByOrgUnit {
      get; internal set;
    }


    public string Status {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }

  }  // class ContractMilestoneDto


  /// Output Dto used to return minimal contract milestone data
  public class ContractMilestoneDescriptor {

    public string ID {
      get; internal set;
    }


    public string UID {
      get; internal set;
    }


    public int Contract {
      get; internal set;
    }


    public string MilestoneNo {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public int Supplier {
      get; internal set;
    }


    public string ManagedByOrgUnit {
      get; internal set;
    }


    public string statusName {
      get; internal set;
    }

  } // class MilestoneDescriptor

}  // namespace Empiria.Contracts.Adapters
