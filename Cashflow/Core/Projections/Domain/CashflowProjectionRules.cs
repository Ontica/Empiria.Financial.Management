/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Service provider                        *
*  Type     : CashFlowProjectionRules                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control cash flow projection's rules.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections {

  /// <summary>Provides services to control cash flow projection's rules.</summary>
  internal class CashFlowProjectionRules {

    #region Fields

    static internal readonly string CASH_FLOW_AUTHORIZER = "cash-flow-authorizer";
    static internal readonly string CASH_FLOW_MANAGER = "cash-flow-manager";
    static internal readonly string CASH_FLOW_PROJECTOR = "cash-flow-projector";
    static internal readonly string CASH_FLOW_ROLE = "cash-flow";

    private readonly CashFlowProjection _projection;

    #endregion Fields

    #region Constructors and parsers

    internal CashFlowProjectionRules(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      _projection = projection;
    }

    #endregion Constructors and parsers

    #region Properties

    public bool CanAuthorize {
      get {
        if (_projection.Status != TransactionStatus.OnAuthorization) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_AUTHORIZER) ||
            ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_MANAGER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanClose {
      get {
        if (_projection.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanDelete {
      get {
        if (_projection.Status != TransactionStatus.Pending) {
          return false;
        }

        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id, _projection.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanEditDocuments {
      get {
        if (_projection.Status != TransactionStatus.Authorized) {
          return false;
        }

        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id, _projection.RecordedBy.Id,
                                                _projection.AppliedBy.Id, _projection.AuthorizedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanReject {
      get {
        if (_projection.Status != TransactionStatus.OnAuthorization &&
            _projection.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_AUTHORIZER) ||
            ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_MANAGER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanSendToAuthorization {
      get {
        if (_projection.Status != TransactionStatus.Pending) {
          return false;
        }

        if (_projection.Entries.Count == 0) {
          return false;
        }
        if (_projection.ProjectionType.IsProtected &&
           (ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_AUTHORIZER) ||
           ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_MANAGER))) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id,
                                                _projection.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanUpdate {
      get {
        if (_projection.IsNew) {
          return true;
        }
        if (_projection.Status != TransactionStatus.Pending) {
          return false;
        }
        if (_projection.ProjectionType.IsProtected &&
            (ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_AUTHORIZER) ||
             ExecutionServer.CurrentPrincipal.IsInRole(CASH_FLOW_MANAGER))) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id,
                                                _projection.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }

    #endregion Properties

  }  // class CashFlowProjectionRules

}  // namespace Empiria.CashFlow.Projections
