/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Input fields DTO                        *
*  Type     : CashFlowProjectionEntryFields              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO for cash flow projections entries.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

using Empiria.Financial;

namespace Empiria.CashFlow.Projections {

  /// <summary>Input fields DTO for cash flow projections entries.</summary>
  public class CashFlowProjectionEntryFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string ProjectionUID {
      get; set;
    } = string.Empty;


    public string CashFlowAccountUID {
      get; set;
    } = string.Empty;


    public string ProjectionColumnUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string ProductUnitUID {
      get; set;
    } = string.Empty;


    public decimal ProductQty {
      get; set;
    } = 1;


    public int? Year {
      get; set;
    }

    public int? Month {
      get; set;
    }

    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public decimal? Amount {
      get; set;
    } = null;

    public decimal? ExchangeRate {
      get; set;
    } = null;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];

  }  // class ProjectionEntryFields


  /// <summary>Extension methods for CashFlowProjectionEntryFields type.</summary>
  static internal class CashFlowProjectionEntryFieldsExtensions {

    static public void EnsureValid(this CashFlowProjectionEntryFields fields) {

      fields.UID = Patcher.CleanUID(fields.UID);
      fields.ProjectionUID = Patcher.CleanUID(fields.ProjectionUID);
      fields.CashFlowAccountUID = Patcher.CleanUID(fields.CashFlowAccountUID);
      fields.ProjectionColumnUID = Patcher.CleanUID(fields.ProjectionColumnUID);
      fields.ProductUID = Patcher.CleanUID(fields.ProductUID);
      fields.ProductUnitUID = Patcher.CleanUID(fields.ProductUnitUID);
      fields.CurrencyUID = Patcher.CleanUID(fields.CurrencyUID);

      fields.Description = EmpiriaString.Clean(fields.Description);
      fields.Justification = EmpiriaString.Clean(fields.Justification);


      if (fields.UID.Length != 0) {
        _ = CashFlowProjectionEntry.Parse(fields.UID);
      }

      if (fields.ProjectionUID.Length != 0) {
        _ = CashFlowProjection.Parse(fields.ProjectionUID);
      }

      if (fields.CashFlowAccountUID.Length != 0) {
        _ = FinancialAccount.Parse(fields.CashFlowAccountUID);
      }

      if (fields.ProjectionColumnUID.Length != 0) {
        _ = CashFlowProjectionColumn.Parse(fields.ProjectionColumnUID);

      }
      if (fields.ProductUnitUID.Length != 0) {
        _ = ProductUnit.Parse(fields.ProductUnitUID);
      }

      if (fields.CurrencyUID.Length != 0) {
        _ = Currency.Parse(fields.CurrencyUID);
      }

      if (fields.ExchangeRate == 0) {
        fields.ExchangeRate = 1;
      }

      if (fields.Month.HasValue) {
        Assertion.Require(1 <= fields.Month.Value && fields.Month.Value <= 12,
                          $"No reconozco el mes proprocionado ({fields.Month.Value}).");

      }

      Assertion.Require(fields.Amount != 0,
                        "El importe debe ser distinto a cero.");

      if (fields.ProductUID.Length != 0) {
        _ = Product.Parse(fields.ProductUID);

        Assertion.Require(fields.ProductUnitUID.Length != 0,
                          "Se requiere la unidad de medida del producto.");
        Assertion.Require(fields.ProductQty > 0,
                          "La cantidad del producto debe ser mayor a cero.");
      }
    }

  }  // class CashFlowProjectionEntryFieldsExtensions

}  // namespace Empiria.CashFlow.Projections
