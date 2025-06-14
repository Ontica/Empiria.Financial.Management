﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Service provider                        *
*  Type     : BudgetTransactionRules                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control budget transaction's rules.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services to control budget transaction's rules.</summary>
  internal class BudgetTransactionRules {

    private readonly BudgetTransaction _transaction;

    internal BudgetTransactionRules(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      _transaction = transaction;
    }

    #region Properties

    public bool CanAuthorize {
      get {
        if (_transaction.Status != TransactionStatus.OnAuthorization) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
            ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer")) {
          return true;
        }
        return false;
      }
    }


    public bool CanClose {
      get {
        if (_transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer")) {
          return true;
        }
        return false;
      }
    }


    public bool CanDelete {
      get {
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }

        if (ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager") &&
            _transaction.BaseParty.Id == ExecutionServer.CurrentContact.Organization.Id) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _transaction.PostedBy.Id, _transaction.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanEditDocuments {
      get {
        if (_transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager") &&
            _transaction.BaseParty.Id == ExecutionServer.CurrentContact.Organization.Id) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _transaction.PostedBy.Id, _transaction.RecordedBy.Id,
                                                _transaction.AppliedBy.Id, _transaction.AuthorizedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanReject {
      get {
        if (_transaction.Status != TransactionStatus.OnAuthorization &&
            _transaction.Status != TransactionStatus.Authorized) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
            ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer")) {
          return true;
        }
        return false;
      }
    }


    public bool CanSendToAuthorization {
      get {
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }

        if (_transaction.Entries.Count == 0) {
          return false;
        }
        if (_transaction.BudgetTransactionType.IsProtected &&
           (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
           ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer"))) {
          return true;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager") &&
            _transaction.BaseParty.Id == ExecutionServer.CurrentContact.Organization.Id) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _transaction.PostedBy.Id, _transaction.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanUpdate {
      get {
        if (_transaction.IsNew) {
          return true;
        }
        if (_transaction.Status != TransactionStatus.Pending) {
          return false;
        }
        if (_transaction.BudgetTransactionType.IsProtected &&
            (ExecutionServer.CurrentPrincipal.IsInRole("budget-manager") ||
             ExecutionServer.CurrentPrincipal.IsInRole("budget-authorizer"))) {
          return true;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole("acquisition-manager") &&
            _transaction.BaseParty.Id == ExecutionServer.CurrentContact.Organization.Id) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _transaction.PostedBy.Id, _transaction.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }

    #endregion Properties

  }  // class BudgetTransactionRules

}  // namespace Empiria.Budgeting.Transactions
