/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestones Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ContractMilestoneItemDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return Contract milestonnes item information.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Contracts.Adapters {

  /// <summary>Data transfer object used to return milestones item information.</summary>
  public class ContractMilestoneItemDto {

    internal ContractMilestoneItemDto() {
      // no op
    }

    public string UID {
      get; internal set;
    }


    public Contract Contract {
      get; internal set;
    }


    public int PaymentNumber {
      get; internal set;
    }


    public DateTime PaymentDate {
      get; internal set;
    }


    public NamedEntityDto PaymentMethod {
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

  }  // class ContractMilestoneItemDto

}  // namespace Empiria.Contracts.Adapters
