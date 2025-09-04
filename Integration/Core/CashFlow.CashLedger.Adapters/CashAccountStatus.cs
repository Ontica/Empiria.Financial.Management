/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.Core.dll        Pattern   : Enumeration                          *
*  Type     : CashAccountStatus                             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Enumerates the cash account status in a cash ledger transaction entry.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Enumerates the cash account status in a cash ledger transaction entry.</summary>
  public enum CashAccountStatus {

    Pending = 0,

    NoCashFlow = -1,

    CashFlowUnassigned = -2,

    CashFlowAssigned = 1,

    All = 255,

    FalsePositive = -3,

  }  // enum CashAccountStatus



  /// <summary>Extension methods for CashAccountStatus type.</summary>
  static public class CashAccountStatusExtensionMethods {

    static public int ControlValue(this CashAccountStatus status) {
      Assertion.Require(status.IsForControl(), nameof(status));

      return (int) status;
    }

    static public bool IsForControl(this CashAccountStatus status) {
      return status == CashAccountStatus.Pending ||
             status == CashAccountStatus.NoCashFlow ||
             status == CashAccountStatus.CashFlowUnassigned;
    }


    static public string Name(this CashAccountStatus status) {

      switch (status) {
        case CashAccountStatus.Pending:
          return "Pendiente";

        case CashAccountStatus.NoCashFlow:
          return "Sin flujo";

        case CashAccountStatus.CashFlowUnassigned:
          return "Con flujo";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled status {status}");
      }
    }

  }  // class CashAccountStatusExtensionMethods

}  // namespace Empiria.CashFlow.CashLedger.Adapters
