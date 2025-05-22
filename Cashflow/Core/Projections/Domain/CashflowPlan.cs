/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashFlowPlan                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cash flow plan.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections {

  /// <summary>Represents a cash flow plan.</summary>
  public class CashFlowPlan : CommonStorage {

    static public readonly int MIN_YEAR = ConfigurationData.Get("cashFlowPlanMinYear", 2025);

    #region Constructors and parsers

    private CashFlowPlan() {
      // Required by Empiria Framework.
    }

    static public CashFlowPlan Parse(int id) => ParseId<CashFlowPlan>(id);

    static public CashFlowPlan Parse(string uid) => ParseKey<CashFlowPlan>(uid);

    static public FixedList<CashFlowPlan> GetList() {
      return BaseObject.GetList<CashFlowPlan>(string.Empty, "Object_Name")
                       .FindAll(x => x.Status != EntityStatus.Deleted)
                       .ToFixedList()
                       .Sort((x, y) => x.StartDate.CompareTo(y.StartDate))
                       .Reverse();
    }


    static public CashFlowPlan Empty => ParseEmpty<CashFlowPlan>();

    #endregion Constructors and parsers

    #region Properties

    public FixedList<CashFlowProjectionCategory> AvailableCategories {
      get {
        return ExtData.GetFixedList<CashFlowProjectionCategory>("availableCategories", false);
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

  }  // class CashFlowPlan

}  // namespace Empiria.CashFlow.Projections
