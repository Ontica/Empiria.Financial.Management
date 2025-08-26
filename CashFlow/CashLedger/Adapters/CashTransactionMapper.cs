/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Mapper                                  *
*  Type     : CashTransactionMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides mapping services for cash ledger transactions.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Provides mapping services for cash ledger transactions.</summary>
  static internal class CashTransactionMapper {

    static internal CashTransactionHolderDto Map(CashTransactionHolderDto transaction) {
      SetCashAccounts(transaction.Entries);
      transaction.Actions = MapActions(transaction);

      return transaction;
    }


    static internal FixedList<CashTransactionHolderDto> Map(FixedList<CashTransactionHolderDto> transactions) {
      return transactions.Select(x => Map(x)).ToFixedList();
    }

    #region Helpers

    static private CashTransactionActions MapActions(CashTransactionHolderDto transaction) {
      return new CashTransactionActions {
        CanAnalize = true,
        CanReview = true,
        CanUpdate = true,
      };
    }


    static private void SetCashAccounts(FixedList<CashTransactionEntryDto> entries) {

      foreach (var entry in entries) {

        entry.CashAccount = new NamedEntityDto(entry.CashAccountId.ToString(),
                                               entry.CashAccountNo);

      }  // foreach

    }

    #endregion Helpers

  }  // class CashTransactionMapper

}  // namespace Empiria.CashFlow.CashLedger.Adapters
