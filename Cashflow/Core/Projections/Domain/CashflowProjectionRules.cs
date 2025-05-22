/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Service provider                        *
*  Type     : CashflowProjectionRules                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control cashflow projection's rules.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Cashflow.Projections {

  /// <summary>Provides services to control cashflow projection's rules.</summary>
  internal class CashflowProjectionRules {

    private readonly CashflowProjection _projection;

    internal CashflowProjectionRules(CashflowProjection projection) {
      Assertion.Require(projection, nameof(projection));

      _projection = projection;
    }

    #region Properties

    public bool CanAuthorize {
      get {
        if (_projection.Status != TransactionStatus.OnAuthorization) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("cashflow-manager") ||
            ExecutionServer.CurrentPrincipal.IsInRole("cashflow-authorizer")) {
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
        if (ExecutionServer.CurrentPrincipal.IsInRole("cashflow-authorizer")) {
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
        if (ExecutionServer.CurrentPrincipal.IsInRole("cashflow-manager") ||
            ExecutionServer.CurrentPrincipal.IsInRole("cashflow-authorizer")) {
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

        if (_projection.ProjectionType.IsProtected &&
           (ExecutionServer.CurrentPrincipal.IsInRole("cashflow-manager") ||
           ExecutionServer.CurrentPrincipal.IsInRole("cashflow-authorizer"))) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id, _projection.RecordedBy.Id })) {
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
            (ExecutionServer.CurrentPrincipal.IsInRole("cashflow-manager") ||
             ExecutionServer.CurrentPrincipal.IsInRole("cashflow-authorizer"))) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _projection.PostedBy.Id, _projection.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }

    #endregion Properties

  }  // class CashflowProjectionRules

}  // namespace Empiria.Cashflow.Projections
