/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contract Milestones Management             Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Fields DTO                              *
*  Type     : ContractMilestoneItemFields                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update contract milestone items information                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

namespace Empiria.Contracts.Adapters {

  /// <summary>DTO fields structure used for update Contract milestone items information.</summary>
  public class ContractMilestoneItemFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string MilestoneUID {
      get; set;
    } = string.Empty;


    public string ContractItemUID {
      get; set;
    } = string.Empty;


    public string ProductItemUID {
      get; set;
    } = string.Empty;


    public decimal Quantity {
      get; set;
    }


    public decimal UnitPrice {
      get; set;
    }

    
    public decimal Total {
      get; set;
    }

    
    internal void EnsureValid() {
      Assertion.Require(MilestoneUID, "Se requiere el identificador del entregable.");
      Assertion.Require(ContractItemUID, "Se requiere el identificador del desglose del contrato.");
      Assertion.Require(ProductItemUID, "Se requiere del número de producto.");
      Assertion.Require(Quantity < 0, "Necesito la cantidad.");
      Assertion.Require(UnitPrice < 0, "Necesito el precio unitario.");

    }

  }  // class ContractMilestoneItemFields

}  // namespace Empiria.Contracts.Adapters
