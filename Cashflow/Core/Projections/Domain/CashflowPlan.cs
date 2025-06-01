/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Aggregate root                          *
*  Type     : CashFlowPlan                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cash flow plan that is an aggregate root for cash flow projections.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using System.Collections.Generic;

using Empiria.Financial;
using Empiria.StateEnums;

using Empiria.CashFlow.Projections.Data;

namespace Empiria.CashFlow.Projections {

  /// <summary>Represents a cash flow plan that is an aggregate root for cash flow projections.</summary>
  public class CashFlowPlan : CommonStorage {

    private Lazy<List<CashFlowProjection>> _projections = new Lazy<List<CashFlowProjection>>();

    #region Constructors and parsers

    private CashFlowPlan() {
      // Required by Empiria Framework.
    }

    static public CashFlowPlan Parse(int id) => ParseId<CashFlowPlan>(id);

    static public CashFlowPlan Parse(string uid) => ParseKey<CashFlowPlan>(uid);

    static public CashFlowPlan Empty => ParseEmpty<CashFlowPlan>();

    static public FixedList<CashFlowPlan> GetList() {
      return GetStorageObjects<CashFlowPlan>()
             .FindAll(x => x.Status != OpenCloseStatus.Deleted)
             .Sort((x, y) => x.StartDate.CompareTo(y.StartDate))
             .Reverse();
    }


    protected override void OnLoad() {
      Reload();
    }

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

    public FixedList<int> Years {
      get {
        return EmpiriaMath.GetRange(StartDate.Year, EndDate.Year)
                          .ToFixedList();
      }
    }

    public OpenCloseStatus Status {
      get {
        return base.GetStatus<OpenCloseStatus>();
      }
    }


    public Currency BaseCurrency {
      get {
        return Currency.Default;
      }
    }


    public FixedList<CashFlowProjection> Projections {
      get {
        return _projections.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    internal CashFlowProjection AddProjection(CashFlowProjectionCategory category,
                                              FinancialAccount baseAccount) {
      Assertion.Require(category, nameof(category));
      Assertion.Require(baseAccount, nameof(baseAccount));
      Assertion.Require(!baseAccount.IsEmptyInstance, nameof(baseAccount));

      AssertCanAddProjection(baseAccount);

      var projection = new CashFlowProjection(this, category, baseAccount);

      _projections.Value.Add(projection);

      return projection;
    }


    internal CashFlowProjection GetProjection(string projectionUID) {
      Assertion.Require(projectionUID, nameof(projectionUID));

      CashFlowProjection projection = _projections.Value.Find(x => x.UID == projectionUID);

      Assertion.Require(projection, $"Projection with UID '{projectionUID}' was not found.");

      return projection;
    }


    internal bool IncludesYear(int year) {
      return Years.Contains(year);
    }


    internal bool IncludesMonth(int year, int month) {
      Assertion.Require(1 <= month && month <= 12, nameof(month));

      var date = new DateTime(year, month, 1);

      return StartDate <= date && date <= EndDate;
    }


    internal void RemoveProjection(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));
      Assertion.Require(Status == OpenCloseStatus.Opened,
                        $"Cash flow plan is not opened. Its status is {Status}.");

      Assertion.Require(_projections.Value.Contains(projection),
                       "Projection to remove does not belong to this cash flow plan.");

      Assertion.Require(projection.Rules.CanDelete,
                        "Projection to remove can not be deleted.");

      projection.DeleteOrCancel();

      _projections.Value.Remove(projection);
    }

    internal CashFlowProjection TryGetProjection(FinancialAccount baseAccount) {

      return _projections.Value.Find(x => x.BaseAccount.Equals(baseAccount));
    }

    #endregion Methods

    #region Helpers

    private void AssertCanAddProjection(FinancialAccount baseAccount) {
      Assertion.Require(!this.IsEmptyInstance, "Can not add projections to the empty instance plan.");

      Assertion.Require(Status == OpenCloseStatus.Opened,
                        $"Cash flow plan is not opened. Its status is {Status}.");

      var orgUnit = baseAccount.OrganizationalUnit;

      Assertion.Require(orgUnit.PlaysRole(CashFlowProjectionRules.CASH_FLOW_ROLE),
                        $"{orgUnit.Name} is not configurated to handle cash flow projections.");

      if (TryGetProjection(baseAccount) != null) {
        Assertion.RequireFail("Ya está registrada una proyección de flujo de efectivo para " +
                              "el mismo proyecto financiero y la misma cuenta.");
      }
    }

    private void Reload() {
      _projections = new Lazy<List<CashFlowProjection>>(() => CashFlowProjectionDataService.GetProjections(this));
    }


    #endregion Helpers

  }  // class CashFlowPlan

}  // namespace Empiria.CashFlow.Projections
