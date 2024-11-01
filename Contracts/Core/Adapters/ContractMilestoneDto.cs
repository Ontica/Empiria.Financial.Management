/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestones Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ContractMilestoneDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return Contract milestones information.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>Data transfer object used to return milestone information.</summary>
  public class ContractMilestoneDto {


    public int ID {
      get; internal set;
    }


    public string UID {
      get; internal set;
    }


    public NamedEntityDto Contract {
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


    public NamedEntityDto Supplier {
      get; internal set;
    }


    public NamedEntityDto ManagedByOrgUnit {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }


    public FixedList<ContractMilestoneItemDto> Items {
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


    public string ContractUID {
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
