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

      return transaction;
    }


    static private void SetCashAccounts(FixedList<CashTransactionEntryDto> entries) {

      foreach (var entry in entries) {

        if (entry.CashAccountId > 0) {
          entry.CashAccount = new NamedEntityDto(entry.CashAccountId.ToString(),
                                                 entry.CashAccountId.ToString());

        } else if (entry.CashAccountId == -2) {
          entry.CashAccount = new NamedEntityDto("-2", "Con flujo");

        } else if (entry.CashAccountId == -1) {
          entry.CashAccount = new NamedEntityDto("-1", "Sin flujo");

        } else if (entry.CashAccountId == 0) {
          entry.CashAccount = new NamedEntityDto("0", "Pendiente");

        }

      }  // foreach

    }

  }  // class CashTransactionMapper

}  // namespace Empiria.CashFlow.CashLedger.Adapters
