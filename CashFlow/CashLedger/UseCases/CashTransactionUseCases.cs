/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Use case interactor class               *
*  Type     : CashLedgerUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrive and manage cash ledger transactions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.Data;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive and manage cash ledger transactions.</summary>
  public class CashTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected CashTransactionUseCases() {
      // no-op
    }

    static public CashTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<CashTransactionHolderDto> AutoCodifyTransaction(long transactionId) {
      Assertion.Require(transactionId > 0, nameof(transactionId));

      CashTransactionHolderDto transaction = await GetTransaction(transactionId);

      var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

      FixedList<CashEntryFields> updatedEntries = processor.Execute();

      if (updatedEntries.Count > 0) {
        transaction = await CashTransactionData.UpdateEntries(updatedEntries);
      }

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<int> AutoCodifyTransactions(string[] transactionIds) {
      Assertion.Require(transactionIds, nameof(transactionIds));

      int counter = 0;

      foreach (var transactionId in transactionIds) {
        CashTransactionHolderDto transaction = await GetTransaction(long.Parse(transactionId));

        var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

        FixedList<CashEntryFields> updatedEntries = processor.Execute();

        if (updatedEntries.Count > 0) {
          _ = await CashTransactionData.UpdateEntries(updatedEntries);
          counter++;
        }
      }

      return counter;
    }


    public async Task<CashTransactionHolderDto> ExecuteCommand(CashEntriesCommand command) {
      Assertion.Require(command, nameof(command));

      CashTransactionHolderDto transaction = await CashTransactionData.GetTransaction(command.TransactionId);

      FixedList<CashEntryFields> updatedEntries = command.GetCashEntriesFields();

      transaction = await CashTransactionData.UpdateEntries(updatedEntries);

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<CashTransactionHolderDto> GetTransaction(long id) {
      Assertion.Require(id > 0, nameof(id));

      CashTransactionHolderDto transaction = await CashTransactionData.GetTransaction(id);

      return CashTransactionMapper.Map(transaction);
    }


    public Task<FileDto> GetTransactionAsPdfFile(long id) {
      Assertion.Require(id > 0, nameof(id));

      return CashTransactionData.GetTransactionAsPdfFile(id);
    }


    public async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      var entries = await CashTransactionData.SearchEntries(query);

      CashTransactionMapper.SetCashAccounts(entries);

      return entries;
    }


    public Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return CashTransactionData.SearchTransactions(query);
    }

    #endregion Use cases

  }  // class CashTransactionUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
