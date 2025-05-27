/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Service provider                        *
*  Type     : FinancialProjectRules                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control financial project's rules.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Projects {

  /// <summary>Provides services to control financial project's rules.</summary>
  internal class FinancialProjectRules {

    #region Fields

    static internal readonly string PROJECT_AUTHORIZER = "financial-project-authorizer";
    static internal readonly string PROJECT_MANAGER = "financial-project-manager";
    static internal readonly string PROJECT_MANGER_ROLE = "cash-flow";

    private readonly FinancialProject _project;

    #endregion Fields

    #region Constructors and parsers

    internal FinancialProjectRules(FinancialProject project) {
      Assertion.Require(project, nameof(project));

      _project = project;
    }

    #endregion Constructors and parsers

    #region Properties

    public bool CanAuthorize {
      get {
        if (_project.Status != EntityStatus.OnReview) {
          return false;
        }
        if (ExecutionServer.CurrentPrincipal.IsInRole(PROJECT_AUTHORIZER) ||
            ExecutionServer.CurrentPrincipal.IsInRole(PROJECT_MANAGER)) {
          return true;
        }
        return false;
      }
    }


    public bool CanDelete {
      get {
        if (_project.Status != EntityStatus.Pending) {
          return false;
        }

        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _project.PostedBy.Id, _project.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanEditDocuments {
      get {
        if (_project.Status == EntityStatus.Deleted || _project.Status == EntityStatus.Suspended) {
          return false;
        }

        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                    new int[] { _project.PostedBy.Id, _project.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }


    public bool CanUpdate {
      get {
        if (_project.IsNew) {
          return true;
        }
        if (_project.Status != EntityStatus.Pending) {
          return false;
        }
        if (_project.FinancialProjectType.IsProtected &&
            (ExecutionServer.CurrentPrincipal.IsInRole(PROJECT_AUTHORIZER) ||
             ExecutionServer.CurrentPrincipal.IsInRole(PROJECT_MANAGER))) {
          return true;
        }
        if (!EmpiriaMath.IsMemberOf(ExecutionServer.CurrentContact.Id,
                                    new int[] { _project.PostedBy.Id,
                                                _project.RecordedBy.Id })) {
          return false;
        }

        return true;
      }
    }

    #endregion Properties

  }  // class FinancialProjectRules

}  // namespace Empiria.Financial.Projects
