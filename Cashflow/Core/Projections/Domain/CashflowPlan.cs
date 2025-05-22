/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashflowPlan                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cashflow plan.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Cashflow.Projections {

  /// <summary>Represents a cashflow plan.</summary>
  public class CashflowPlan : CommonStorage {

    static public readonly int MIN_YEAR = ConfigurationData.Get("cashflowPlanMinYear", 2025);

    #region Constructors and parsers

    private CashflowPlan() {
      // Required by Empiria Framework.
    }

    static public CashflowPlan Parse(int id) => ParseId<CashflowPlan>(id);

    static public CashflowPlan Parse(string uid) => ParseKey<CashflowPlan>(uid);

    static public FixedList<CashflowPlan> GetList() {
      return BaseObject.GetList<CashflowPlan>(string.Empty, "Object_Name")
                       .FindAll(x => x.Status != EntityStatus.Deleted)
                       .ToFixedList()
                       .Sort((x, y) => x.StartDate.CompareTo(y.StartDate))
                       .Reverse();
    }


    static public CashflowPlan Empty => ParseEmpty<CashflowPlan>();

    #endregion Constructors and parsers

    #region Properties

    public FixedList<CashflowProjectionCategory> AvailableCategories {
      get {
        return ExtData.GetFixedList<CashflowProjectionCategory>("availableCategories", false);
      }
    }

    public bool EditionAllowed {
      get {
        return AvailableCategories.Count > 0;
      }
    }


    public string Prefix {
      get {
        if (IsEmptyInstance) {
          return "N/D";
        }
        return base.Code;
      }
    }

    public int[] Years {
      get {
        return EmpiriaMath.GetRange(StartDate.Year, EndDate.Year);
      }
    }

    public EntityStatus Status {
      get {
        return base.GetStatus<EntityStatus>();
      }
    }

    #endregion Properties

  }  // class CashflowPlan

}  // namespace Empiria.Cashflow.Projections
