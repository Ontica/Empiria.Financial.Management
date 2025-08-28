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

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive and manage cash ledger transactions.</summary>
  public class CashTransactionUseCases : UseCase {

    private readonly CashTransactionServices _financialAccountingServices;

    #region Constructors and parsers

    protected CashTransactionUseCases() {
      _financialAccountingServices = new CashTransactionServices();
    }

    static public CashTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<FixedList<CashTransactionAnalysisEntry>> AnalyzeTransaction(long transactionId) {
      Assertion.Require(transactionId > 0, nameof(transactionId));

      CashTransactionHolderDto transaction = await GetTransaction(transactionId);

      var analyzer = new CashTransactionAnalyzer(transaction.Entries);

      FixedList<CashTransactionAnalysisEntry> entries = analyzer.Execute();

      return entries;
    }


    public async Task<CashTransactionHolderDto> AutoCodifyTransaction(long transactionId) {
      Assertion.Require(transactionId > 0, nameof(transactionId));

      CashTransactionHolderDto transaction = await GetTransaction(transactionId);

      var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

      FixedList<CashEntryFields> updatedEntries = processor.Execute();

      if (updatedEntries.Count > 0) {
        transaction = await _financialAccountingServices.UpdateEntries<CashTransactionHolderDto>(updatedEntries);
      }

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<int> AutoCodifyTransactions(FixedList<long> transactionIds) {
      Assertion.Require(transactionIds, nameof(transactionIds));

      int counter = 0;
      int CHUNK_SIZE = 200;

      FixedList<long>[] chunks = transactionIds.Split(CHUNK_SIZE);

      foreach (FixedList<long> chunk in chunks) {

        var transactions = await _financialAccountingServices.GetTransactions<CashTransactionHolderDto>(chunk);

        var chunkEntries = new List<CashEntryFields>(chunk.Count * 32);

        foreach (var transaction in transactions) {
          var processor = new CashTransactionProcessor(transaction.Transaction, transaction.Entries);

          FixedList<CashEntryFields> updatedTransactionEntries = processor.Execute();

          if (updatedTransactionEntries.Count > 0) {
            chunkEntries.AddRange(updatedTransactionEntries);
            EmpiriaLog.Debug($"Rare {transaction.Transaction.Number}");
            counter++;
          }

        }  // foreach transaction

        if (chunkEntries.Count > 0) {
          await _financialAccountingServices.UpdateBulkEntries(chunkEntries.ToFixedList());
        }

      }  // foreach chunk

      return counter;
    }


    public async Task<CashTransactionHolderDto> ExecuteCommand(CashEntriesCommand command) {
      Assertion.Require(command, nameof(command));

      var transaction =
            await _financialAccountingServices.GetTransaction<CashTransactionHolderDto>(command.TransactionId);

      FixedList<CashEntryFields> updatedEntries = command.GetCashEntriesFields();

      transaction = await _financialAccountingServices.UpdateEntries<CashTransactionHolderDto>(updatedEntries);

      return CashTransactionMapper.Map(transaction);
    }


    public async Task<CashTransactionHolderDto> GetTransaction(long id) {
      Assertion.Require(id > 0, nameof(id));

      var transaction = await _financialAccountingServices.GetTransaction<CashTransactionHolderDto>(id);

      return CashTransactionMapper.Map(transaction);
    }


    public Task<FileDto> GetTransactionAsPdfFile(long id) {
      Assertion.Require(id > 0, nameof(id));

      return _financialAccountingServices.GetTransactionAsPdfFile(id);
    }


    public async Task<FixedList<CashTransactionHolderDto>> GetTransactions(FixedList<long> transactionIds) {
      Assertion.Require(transactionIds, nameof(transactionIds));

      var transactions = await _financialAccountingServices.GetTransactions<CashTransactionHolderDto>(transactionIds);

      return CashTransactionMapper.Map(transactions);
    }


    public async Task<FixedList<CashEntryDescriptor>> GetTransactionsEntries(FixedList<long> entriesIds) {
      Assertion.Require(entriesIds, nameof(entriesIds));

      return await _financialAccountingServices.GetTransactionsEntries(entriesIds);
    }


    public async Task<FixedList<CashEntryDescriptor>> SearchEntries(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return await _financialAccountingServices.SearchEntries(query);
    }


    public Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return _financialAccountingServices.SearchTransactions(query);
    }

    #endregion Use cases

  }  // class CashTransactionUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
