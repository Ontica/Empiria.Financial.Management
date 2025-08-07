/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Command DTO                             *
*  Type     : CashEntriesCommand                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input command used to update cash transaction entries.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  public enum CashEntryOperation {

    MarkAsCashEntries = 1,

    MarkAsCashEntriesPending = 0,

    MarkAsNoCashEntries = 2,

    RemoveCashEntries = 3,

  };


  /// <summary>Input command used to update cash transaction entries.</summary>
  public class CashEntriesCommand {

    public CashEntryOperation Operation {
      get; set;
    }


    public long[] Entries {
      get; set;
    } = new long[0];


    public string CashAccount {
      get; set;
    } = string.Empty;


    public int CashAccountId {
      get; set;
    }


    internal void SetCashAccountId() {
      switch (Operation) {

        case CashEntryOperation.MarkAsCashEntries:

          if (string.IsNullOrWhiteSpace(CashAccount)) {
            CashAccountId = -2;
          } else if (EmpiriaString.IsInteger(CashAccount)) {
            CashAccountId = int.Parse(CashAccount);
          } else {
            Assertion.RequireFail($"No tengo registrado el concepto presupuestal {CashAccount}.");
          }

          return;


        case CashEntryOperation.MarkAsCashEntriesPending:
          CashAccountId = -2;
          return;

        case CashEntryOperation.MarkAsNoCashEntries:
          CashAccountId = -1;
          return;

        case CashEntryOperation.RemoveCashEntries:
          CashAccountId = 0;
          return;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized cash entry operation '{Operation.ToString()}'");
      }
    }

  }  // CashEntriesCommand

}  // namespace Empiria.CashFlow.CashLedger.Adapters
