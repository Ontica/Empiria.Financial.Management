/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashFlowProjectionEntryByYear              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a cash flow proyection entry with values for a whole year.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Financial;
using Empiria.Products;

namespace Empiria.CashFlow.Projections {

  /// <summary>Holds a cash flow proyection entry with values for a whole year.</summary>
  public class CashFlowProjectionEntryByYear {

    public CashFlowProjectionEntryByYear(string entryByYearUID, FixedList<CashFlowProjectionEntry> entries) {
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));
      Assertion.Require(entries, nameof(entries));
      Assertion.Require(entries.Count > 0, "entries must not be an empty list.");

      CashFlowProjectionEntryByYearFields fields = CashFlowProjectionByYear.BuildFields(entryByYearUID);

      Projection = CashFlowProjection.Parse(fields.ProjectionUID);
      ProjectionColumn = CashFlowProjectionColumn.Parse(fields.ProjectionColumnUID);
      CashFlowAccount = FinancialAccount.Parse(fields.CashFlowAccountUID);
      Product = Product.Parse(fields.ProductUID);
      ProductUnit = ProductUnit.Parse(fields.ProductUnitUID);
      Currency = Currency.Parse(fields.CurrencyUID);
      Year = fields.Year;

      Entries = entries;
    }

    #region Properties

    public string UID {
      get {
        return CashFlowProjectionByYear.BuildUID(this);
      }
    }

    public CashFlowProjection Projection {
      get;
    }

    public CashFlowProjectionColumn ProjectionColumn {
      get;
    }

    public FinancialAccount CashFlowAccount {
      get;
    }

    public Product Product {
      get;
    }

    public string Description {
      get;
    } = string.Empty;


    public ProductUnit ProductUnit {
      get; private set;
    }


    public string Justification {
      get {
        var modes = Entries.GetModes(x => x.Justification);

        return modes.Count == 1 ? modes[0] : string.Empty;
      }
    }


    public int Year {
      get;
    }

    public Currency Currency {
      get;
    }

    public FixedList<CashFlowProjectionEntry> Entries {
      get; private set;
    }

    public decimal Total {
      get {
        return Entries.Sum(x => x.Amount);
      }
    }

    #endregion Properties

    #region Methods

    public decimal GetAmountForMonth(int month) {
      return Entries.FindAll(x => x.Month == month)
                    .Sum(x => x.Amount);
    }

    #endregion Methods

  }  // CashFlowProjectionEntryByYear

}  // namespace Empiria.CashFlow.Projections
