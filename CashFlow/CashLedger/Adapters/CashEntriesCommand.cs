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

    #region Properties

    public CashEntryOperation Operation {
      get; set;
    }

    public long TransactionId {
      get; set;
    }


    public long[] Entries {
      get; set;
    } = new long[0];


    public string CashAccount {
      get; set;
    } = string.Empty;

    #endregion Properties

    #region Methods

    internal FixedList<CashEntryFields> GetCashEntriesFields() {
      return Entries.ToFixedList()
                    .Select(x => ToCashEntryFields(x))
                    .ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private CashAccountStatus GetCashAccountStatus() {
      switch (Operation) {

        case CashEntryOperation.MarkAsCashEntries:

          if (string.IsNullOrWhiteSpace(CashAccount)) {
            return CashAccountStatus.CashFlowUnassigned;

          } else if (EmpiriaString.IsInteger(CashAccount)) {
            return CashAccountStatus.CashFlowAssigned;

          } else {
            throw Assertion.EnsureNoReachThisCode($"No tengo registrado el concepto presupuestal {CashAccount}.");
          }

        case CashEntryOperation.MarkAsCashEntriesPending:
          return CashAccountStatus.CashFlowUnassigned;

        case CashEntryOperation.MarkAsNoCashEntries:
          return CashAccountStatus.NoCashFlow;

        case CashEntryOperation.RemoveCashEntries:
          return CashAccountStatus.Pending;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized cash entry operation '{Operation.ToString()}'");
      }
    }


    private CashEntryFields ToCashEntryFields(long entryId) {
      CashAccountStatus status = GetCashAccountStatus();

      int accountId = status == CashAccountStatus.CashFlowAssigned ?
                                        int.Parse(CashAccount) : status.ControlValue();

      string accountNo = status == CashAccountStatus.CashFlowAssigned ?
                                        CashAccount : status.Name();

      return new CashEntryFields {
        EntryId = entryId,
        TransactionId = this.TransactionId,
        CashAccountId = accountId,
        CashAccountNo = accountNo,
        CashFlowRecordedById = status != CashAccountStatus.Pending ? ExecutionServer.CurrentUserId : -1
      };
    }

    #endregion Helpers

  }  // CashEntriesCommand

}  // namespace Empiria.CashFlow.CashLedger.Adapters
