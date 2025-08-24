/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Use case interactor class               *
*  Type     : CashLedgerUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrive and manage cash ledger transactions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.Services;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.Data;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive and manage cash ledger transactions.</summary>
  public class CashTransactionUseCases : UseCase {

    private readonly CashTransactionData _cashTransactionData;

    #region Constructors and parsers

    protected CashTransactionUseCases() {
      _cashTransactionData = new CashTransactionData();
    }

    static public CashTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<FixedList<CashTransactionAnalysisEntry>> AnalyzeTransaction(long transactionId) {
      Assertion.Require(transactionId > 0, nameof(transactionId));

      CashTransactionHolderDto transaction = await GetTransaction(transactionId, false);

      var analyzer = new CashTransactionAnalyzer(transaction.Entries);

      FixedList<CashTransactionAnalysisEntry> entries = analyzer.Execute();

      return entries;
    }


    public async Task<CashTransactionHolderDto> AutoCodifyTransaction(long transactionId) {
      Assertion.Require(transactionId > 0, nameof(transactionId));

      CashTransactionHolderDto transaction = await GetTransaction(transactionId, false);

      var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

      FixedList<CashEntryFields> updatedEntries = processor.Execute();

      if (updatedEntries.Count > 0) {
        transaction = await _cashTransactionData.UpdateEntries(updatedEntries);
      }

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<int> AutoCodifyTransactions(FixedList<long> transactionIds) {
      Assertion.Require(transactionIds, nameof(transactionIds));

      int counter = 0;
      int CHUNK_SIZE = 200;

      FixedList<long>[] chunks = transactionIds.Split(CHUNK_SIZE);

      foreach (FixedList<long> chunk in chunks) {

        FixedList<CashTransactionHolderDto> transactions =
                    await _cashTransactionData.GetTransactions(chunk, false);

        var chunkEntries = new List<CashEntryFields>(chunk.Count * 8);

        foreach (var transaction in transactions) {
          var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

          FixedList<CashEntryFields> updatedTransactionEntries = processor.Execute();

          if (updatedTransactionEntries.Count > 0) {
            chunkEntries.AddRange(updatedTransactionEntries);
            counter++;
          }
        }

        if (chunkEntries.Count > 0) {
          await _cashTransactionData.UpdateBulkEntries(chunkEntries.ToFixedList());
        }
      }

      return counter;
    }


    public async Task<CashTransactionHolderDto> ExecuteCommand(CashEntriesCommand command) {
      Assertion.Require(command, nameof(command));

      CashTransactionHolderDto transaction =
                    await _cashTransactionData.GetTransaction(command.TransactionId, false);

      FixedList<CashEntryFields> updatedEntries = command.GetCashEntriesFields();

      transaction = await _cashTransactionData.UpdateEntries(updatedEntries);

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<CashTransactionHolderDto> GetTransaction(long id, bool returnLegacySystemData) {
      Assertion.Require(id > 0, nameof(id));

      CashTransactionHolderDto transaction = await _cashTransactionData.GetTransaction(id, returnLegacySystemData);

      return CashTransactionMapper.Map(transaction);
    }


    public Task<FileDto> GetTransactionAsPdfFile(long id) {
      Assertion.Require(id > 0, nameof(id));

      return _cashTransactionData.GetTransactionAsPdfFile(id);
    }


    public async Task<FixedList<CashTransactionHolderDto>> GetTransactions(FixedList<long> transactionIds,
                                                                           bool returnLegacySystemData) {
      Assertion.Require(transactionIds, nameof(transactionIds));

      FixedList<CashTransactionHolderDto> transactions =
                          await _cashTransactionData.GetTransactions(transactionIds, returnLegacySystemData);

      return CashTransactionMapper.Map(transactions);
    }


    public async Task<FixedList<CashEntryDescriptor>> GetTransactionsEntries(FixedList<long> entriesIds) {
      Assertion.Require(entriesIds, nameof(entriesIds));

      FixedList<CashEntryDescriptor> entries = await _cashTransactionData.GetTransactionsEntries(entriesIds);

      CashTransactionMapper.SetCashAccounts(entries);

      return entries;
    }


    public async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      var entries = await _cashTransactionData.SearchEntries(query);

      CashTransactionMapper.SetCashAccounts(entries);

      return entries;
    }


    public Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return _cashTransactionData.SearchTransactions(query);
    }

    #endregion Use cases

  }  // class CashTransactionUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
