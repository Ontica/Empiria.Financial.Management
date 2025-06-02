/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashFlowProjectionTotalByYear              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a cash flow calculated projection entry with total values for a whole year.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

namespace Empiria.CashFlow.Projections {

  /// <summary>Holds a cash flow calculated projection entry with total values for a whole year.</summary>
  public class CashFlowProjectionTotalByYear {

    public CashFlowProjectionTotalByYear(CashFlowProjection projection, int year,
                                         CashFlowProjectionColumn column,
                                         FixedList<CashFlowProjectionEntry> entries) {

      Assertion.Require(projection, nameof(projection));
      Assertion.Require(column, nameof(column));
      Assertion.Require(entries, nameof(entries));
      Assertion.Require(entries.Count > 0, nameof(entries));

      Projection = projection;
      ProjectionColumn = column;
      Year = year;

      Entries = entries;
    }

    #region Properties


    public CashFlowProjection Projection {
      get;
    }

    public CashFlowProjectionColumn ProjectionColumn {
      get;
    }

    public int Year {
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

    public decimal GetTotalForMonth(int month) {
      return Entries.FindAll(x => x.Month == month)
                    .Sum(x => x.Amount);
    }

    #endregion Methods

  }  // CashFlowProjectionTotalByYear

}  // namespace Empiria.CashFlow.Projections
