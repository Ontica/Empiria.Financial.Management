/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestone Management              Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Fields DTO                              *
*  Type     : ContractMilestoneFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used for update contract milestone information.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Contracts.Adapters {

  /// <summary>Input fields DTO used for update contract milestones information.</summary>
  public class ContractMilestoneFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string ContractUID {
      get; set;
    } = string.Empty;


    public int PaymentNumber {
      get; set;
    }


    public DateTime PaymentDate {
      get; set;
    }


    public string ManagedByOrgUnitUID {
      get; set;
    }


    public string Status {
      get; set;
    } = string.Empty;


    public decimal Total {
      get; set;
    } = 0.00m;


    internal void EnsureValid() {
      Assertion.Require(PaymentDate, "Se requiere la fecha del pago.");
      Assertion.Require(PaymentNumber, "Se requiere el numero de pago.");
    }

  }  // class ContractMilestoneFields

}  // namespace Empiria.Contracts.Adapters
