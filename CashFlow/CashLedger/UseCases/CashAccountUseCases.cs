/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Use case interactor class               *
*  Type     : CashAccountUseCases                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrive cash accounts transactions.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.Services;
using Empiria.Storage;

using Empiria.Financial.Adapters;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive cash accounts transactions.</summary>
  public class CashAccountUseCases : UseCase {

    private readonly CashTransactionServices _financialAccountingServices;

    #region Constructors and parsers

    protected CashAccountUseCases() {
      _financialAccountingServices = new CashTransactionServices();
    }

    static public CashAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashAccountUseCases>();
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
            // EmpiriaLog.Debug($"Rare {transaction.Transaction.Number}");
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


    public async Task<FixedList<CashAccountTotalDto>> SearchAccounts(AccountsQuery query) {
      Assertion.Require(query, nameof(query));

      // AccountsQuery adaptedQuery = CashAccountMapper.Map(query);

      // accounts _financialAccountingServices.SearchAccounts(adaptedQuery);

      // return CashAccountMapper.Map(accounts);

      return await Task.FromResult(new FixedList<CashAccountTotalDto>());
    }

    #endregion Use cases

  }  // class CashAccountUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
