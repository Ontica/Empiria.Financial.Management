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

namespace Empiria.Contracts.Adapters
{

    /// <summary>DTO fields structure used for update Contract milestone items information.</summary>
    public class ContractMilestoneItemFields
    {

        public string UID
        {
            get; set;
        } = string.Empty;

        public string MilestoneUID
        {
            get; set;
        } = string.Empty;

        public string ContractItemUID
        {
            get; set;
        } = string.Empty;

        public string Description
        {
            get; set;
        } = string.Empty;

        public decimal Quantity
        {
            get; set;
        }

        public string ProductUnitUID
        {
            get; set;
        }

        public string ProductUID
        {
            get; set;
        } = string.Empty;

        public decimal UnitPrice
        {
            get; set;
        }
        public string BudgetAccountUID
        {
            get; set;
        } = string.Empty;

        public string Status
        {
            get; set;
        } = string.Empty;

        public decimal Total
        {
            get; set;
        }

        internal void EnsureValid()
        {
            Assertion.Require(MilestoneUID, "Se requiere el identificador del entregable.");
            Assertion.Require(ContractItemUID, "Se requiere el identificador del concepto del entregable.");
            Assertion.Require(Quantity, "Se requiere la cantidad del prodcuto.");
            Assertion.Require(ProductUnitUID, "Se requiere la unidad del producto.");
            Assertion.Require(ProductUID, "Se requiere del número de producto.");
            Assertion.Require(Quantity > 0, "Se requiere la cantidad del producto.");
            Assertion.Require(UnitPrice > 0, "Se requiere el precio unitario del producto.");
            Assertion.Require(BudgetAccountUID, "Se requiere la clave presupuestal del producto.");
        }

    }  // class ContractMilestoneItemFields

}  // namespace Empiria.Contracts.Adapters
