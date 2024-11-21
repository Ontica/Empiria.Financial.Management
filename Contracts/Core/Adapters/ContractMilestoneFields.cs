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
    }


    public string MilestoneNo {
      get; set;
    }


    public string Name {
      get; set;
    }


    public string Description {
      get; set;
    }


    public string SupplierUID {
      get; set;
    }


    public string PaymentExtData {
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
      Assertion.Require(MilestoneNo, "Se requiere el numero del entregable.");
      Assertion.Require(Name, "Se requiere el nombre del entregable.");
      Assertion.Require(Description, "Se requiere la descripción del entregable.");
      Assertion.Require(SupplierUID, "Se requiere el proveedor del entregable.");

    }

  }  // class ContractMilestoneFields

}  // namespace Empiria.Contracts.Adapters
