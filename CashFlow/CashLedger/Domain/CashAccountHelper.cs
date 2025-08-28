/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Static services library                 *
*  Type     : CashAccountHelper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services for cash accounts.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger {

  /// <summary>Provides services for cash accounts.</summary>
  internal class CashAccountHelper {

    static internal string GetCashAccountNo(int cashAccountId) {

      if (cashAccountId == -1) {
        return "Sin flujo";

      } else if (cashAccountId == 0) {
        return "Pendiente";

      } else if (cashAccountId == -2) {
        return "Con flujo";
      }

      throw Assertion.EnsureNoReachThisCode($"Unrecognized CashAccountId {cashAccountId}");
    }

  }  // class CashAccountHelper

}  // namespace Empiria.CashFlow.CashLedger
