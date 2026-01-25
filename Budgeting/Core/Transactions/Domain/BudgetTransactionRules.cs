/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Service provider                        *
*  Type     : BudgetTransactionRules                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control budget transaction's rules.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.HumanResources;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Provides services to control budget transaction's rules.</summary>
  internal class BudgetTransactionRules {

    #region Fields

    static internal readonly string ACQUISITION_MANAGER = "acquisition-manager";
    static internal readonly string BUDGET_AUTHORIZER = "budget-authorizer";
    static internal readonly string BUDGET_MANAGER = "budget-manager";

    private readonly BudgetTransaction _transaction;
    private readonly Party _currentUser;
    private readonly FixedList<string> _securityRoles;
    private readonly FixedList<string> _userRoles;
    private readonly FixedList<OrganizationalUnit> _acquisitionOrgUnits;

    #endregion Fields

    #region Constructors and parsers

    internal BudgetTransactionRules(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      _transaction = transaction;

      _currentUser = Party.ParseWithContact(ExecutionServer.CurrentContact);

      _securityRoles = _currentUser.GetSecurityRoles()
                                   .SelectDistinct(x => x.NamedKey);

      _userRoles = Accountability.GetResponsibleRoles(_currentUser);

      _acquisitionOrgUnits = GetUserAcquisitionOrgUnits();
    }


    static internal FixedList<OrganizationalUnit> GetUserAcquisitionOrgUnits() {

      var currentUser = Party.ParseWithContact(ExecutionServer.CurrentContact);

      return Accountability.GetCommissionersFor<OrganizationalUnit>(currentUser, "budgeting", ACQUISITION_MANAGER);
    }


    #endregion Constructors and parsers

    #region Properties

    public bool CanAuthorize {
      get {
        if (_transaction.Status != TransactionStatus.OnAuthorization) {
          return false;
        }
        if (_securityRoles.Contains(BUDGET_AUTHORIZER) || _securityRoles.Contains(BUDGET_MANAGER)) {
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
        if (_securityRoles.Contains(BUDGET_MANAGER)) {
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

        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.BaseParty))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) ||
            _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }
        if (_securityRoles.Contains(BUDGET_MANAGER) ||
            _securityRoles.Contains(BUDGET_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanEditDocuments {
      get {
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.BaseParty))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) ||
            _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }
        if (_securityRoles.Contains(BUDGET_MANAGER) ||
          _securityRoles.Contains(BUDGET_AUTHORIZER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanReject {
      get {
        if (_transaction.Status == TransactionStatus.OnAuthorization &&
            _userRoles.Contains(BUDGET_AUTHORIZER)) {
          return true;
        }
        if (_transaction.Status == TransactionStatus.Authorized &&
          _userRoles.Contains(BUDGET_MANAGER)) {
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
        if (_transaction.TransactionType.IsProtected &&
            _userRoles.Contains(BUDGET_MANAGER) ||
            _userRoles.Contains(BUDGET_AUTHORIZER)) {
          return true;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.BaseParty))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }

        return false;
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
        if (_transaction.TransactionType.IsProtected &&
            _userRoles.Contains(BUDGET_MANAGER) ||
            _userRoles.Contains(BUDGET_AUTHORIZER)) {
          return true;
        }
        if (_userRoles.Contains(ACQUISITION_MANAGER) &&
            _acquisitionOrgUnits.Contains(x => x.Equals(_transaction.BaseParty))) {
          return true;
        }
        if (_currentUser.Equals(_transaction.PostedBy) || _currentUser.Equals(_transaction.RecordedBy)) {
          return true;
        }

        return false;
      }
    }

    #endregion Properties

  }  // class BudgetTransactionRules

}  // namespace Empiria.Budgeting.Transactions
