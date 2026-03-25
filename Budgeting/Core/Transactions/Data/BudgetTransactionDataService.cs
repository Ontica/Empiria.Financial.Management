/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Data Layer                              *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Data Service                            *
*  Type     : BudgetTransactionDataService               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for budget transactions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Data;

using Empiria.Financial;

namespace Empiria.Budgeting.Transactions.Data {

  /// <summary>Provides data access services for budget transactions.</summary>
  static public class BudgetTransactionDataService {

    static internal void CleanEntries(BudgetEntry entry) {
      if (entry.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_BUDGET_ENTRIES " +
               $"SET BDG_ENTRY_UID = '{Guid.NewGuid().ToString()}', " +
               $"BDG_ENTRY_KEYWORDS = '{entry.Keywords}' " +
               $"WHERE BDG_ENTRY_ID = {entry.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static public void CleanTransaction(BudgetTransaction transaction) {
      if (transaction.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_BUDGET_TRANSACTIONS " +
               $"SET BDG_TXN_KEYWORDS = '{transaction.Keywords}' " +
               $"WHERE BDG_TXN_ID = {transaction.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal void CopyRelatedEntryControlCodes(BudgetTransaction transaction) {

      foreach (var entry in transaction.Entries.FindAll(x => x.RelatedEntryId > 0)) {

        var relatedEntry = BudgetEntry.Parse(entry.RelatedEntryId);

        entry.ControlNo = relatedEntry.ControlNo;

        entry.Save();
      }
    }


    static internal void GenerateApprovedPaymentControlCodes(BudgetTransaction transaction) {

      var paymentNos = BudgetTransaction.GetRelatedTo(transaction)
                                        .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                                      (x.InProcess || x.IsClosed))
                                        .SelectDistinctFlat(x => x.Entries.SelectDistinct(y => y.ControlNo))
                                        .FindAll(x => x.Length > 0 && x.Contains("/"))
                                        .Sort((x, y) => x.CompareTo(y))
                                        .Reverse();

      int counter = 0;

      if (paymentNos.Count > 0) {
        var paymentNo = paymentNos[0].Split('/')[1];
        counter = int.Parse(paymentNo);
        counter++;
      } else {
        counter = 1;
      }

      foreach (var entry in transaction.Entries.FindAll(x => x.RelatedEntryId > 0)) {

        var relatedEntry = BudgetEntry.Parse(entry.RelatedEntryId);

        string paymentNoString = counter.ToString("00");

        entry.ControlNo = $"{relatedEntry.ControlNo}/{paymentNoString}";

        entry.Save();
      }
    }


    static internal void GenerateRequestControlCodes(BudgetTransaction transaction) {

      foreach (var year in transaction.Entries.SelectDistinct(x => x.Year)) {

        var entriesForYear = transaction.Entries.FindAll(x => x.Year == year);

        foreach (var account in entriesForYear.SelectDistinct(x => x.BudgetAccount)) {

          var entriesForAccountAndYear = entriesForYear.FindAll(x => x.BudgetAccount.Equals(account));

          var controlNo = GetNextControlNo(year);

          foreach (var entry in entriesForAccountAndYear) {
            entry.ControlNo = controlNo;
            entry.Save();
          }
        }
      }
    }


    static private string GetNextControlNo(int requestYear) {

      string prefix = $"{requestYear.ToString().Substring(2)}";

      string sql = "SELECT MAX(BDG_ENTRY_CONTROL_NO) " +
                   "FROM FMS_BUDGET_ENTRIES " +
                   $"WHERE BDG_ENTRY_CONTROL_NO LIKE '{prefix}-%' AND " +
                   $"BDG_ENTRY_CONTROL_NO NOT LIKE '{prefix}-%/%'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), string.Empty);

      if (lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID.Split('-')[1]) + 1;

        return $"{prefix}-{consecutive:00000}";

      } else {
        return $"{prefix}-00001";
      }
    }


    static internal string GetNextTransactionNo(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      if (transaction.HasTransactionNo) {
        return transaction.TransactionNo;
      }

      string transactionPrefix = transaction.TransactionType.Prefix;
      string budgetPrefix = transaction.BaseBudget.BudgetType.Prefix;

      int year = transaction.BaseBudget.Year;

      string prefix = $"{year}-{transactionPrefix}-{budgetPrefix}";

      string sql = "SELECT MAX(BDG_TXN_NUMBER) " +
                   "FROM FMS_BUDGET_TRANSACTIONS " +
                   $"WHERE BDG_TXN_NUMBER LIKE '{prefix}-%'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), string.Empty);

      if (lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID.Split('-')[3]) + 1;

        return $"{prefix}-{consecutive:00000}";

      } else {
        return $"{prefix}-00001";
      }
    }


    static internal FixedList<BudgetTransaction> GetRelatedTransactions(BudgetTransaction transaction) {
      FixedList<int> entryIds;

      string controlNosFilter = string.Empty;



      if (transaction.OperationType == BudgetOperationType.Request) {
        entryIds = transaction.Entries.SelectDistinct(x => x.Id);

        entryIds = FixedList<int>.MergeDistinct(entryIds,
                                                entryIds.SelectDistinct(x => BudgetEntry.Parse(x).RelatedEntryId)
                                                                                        .FindAll(x => x > 0));

        entryIds = FixedList<int>.MergeDistinct(entryIds,
                                                entryIds.SelectDistinct(x => BudgetEntry.Parse(x).RelatedEntryId)
                                                                                        .FindAll(x => x > 0));

        var controlNos = entryIds.Select(x => BudgetEntry.Parse(x)).ToFixedList().SelectDistinct(x => x.ControlNo).FindAll(x => x.Length > 0);

        foreach (var controlNo in controlNos) {
          if (controlNosFilter.Length > 0) {
            controlNosFilter += " OR ";
          }
          controlNosFilter += $" BDG_ENTRY_CONTROL_NO LIKE '{controlNo}%'";
        }

        if (controlNosFilter.Length > 0) {
          controlNosFilter = $"({controlNosFilter}) OR ";
        }

      } else {
        entryIds = transaction.Entries.SelectDistinct(x => x.RelatedEntryId)
                                      .FindAll(x => x > 0);

        entryIds = FixedList<int>.MergeDistinct(entryIds,
                                                entryIds.SelectDistinct(x => BudgetEntry.Parse(x).RelatedEntryId)
                                                                                        .FindAll(x => x > 0));

        entryIds = FixedList<int>.MergeDistinct(entryIds,
                                                entryIds.SelectDistinct(x => BudgetEntry.Parse(x).RelatedEntryId)
                                                                                        .FindAll(x => x > 0));

        var controlNos = entryIds.Select(x => BudgetEntry.Parse(x)).ToFixedList().SelectDistinct(x => x.ControlNo).FindAll(x => x.Length > 0);

        foreach (var controlNo in controlNos) {
          if (controlNosFilter.Length > 0) {
            controlNosFilter += " OR ";
          }
          controlNosFilter += $" BDG_ENTRY_CONTROL_NO LIKE '{controlNo}%'";
        }

        if (controlNosFilter.Length > 0) {
          controlNosFilter = $"({controlNosFilter}) OR ";
        }

      }

      if (entryIds.Count == 0) {
        return FixedList<BudgetTransaction>.Empty;
      }

      string filter = "BDG_TXN_ID IN (SELECT DISTINCT BDG_ENTRY_TXN_ID " +
                               "FROM FMS_BUDGET_ENTRIES WHERE " +
                               $"(BDG_ENTRY_ID IN ({string.Join(",", entryIds)}) OR " +
                                controlNosFilter +
                                $"BDG_ENTRY_RELATED_ENTRY_ID IN ({string.Join(",", entryIds)})) AND " +
                               $"BDG_ENTRY_STATUS NOT IN ('J', 'X'))";

      return SearchTransactions(filter, "BDG_TXN_POSTING_TIME");
    }


    static internal FixedList<BudgetTransaction> GetTransactions(IBudgetable budgetable) {
      Assertion.Require(budgetable, nameof(budgetable));

      var filter = $"BDG_TXN_ENTITY_TYPE_ID = {budgetable.Data.BudgetableType.Id} AND " +
                   $"BDG_TXN_ENTITY_ID = {budgetable.Id} AND " +
                   $"BDG_TXN_STATUS <> 'X'";

      return SearchTransactions(filter, "BDG_TXN_ID");
    }


    static internal FixedList<BudgetTransaction> GetTransactions(Budget budget,
                                                                 BudgetTransactionType transactionType) {
      var sql = "SELECT * FROM VW_FMS_BUDGET_TRANSACTIONS " +
               $"WHERE BDG_TXN_TYPE_ID = {transactionType.Id} AND " +
               $"BDG_TXN_BASE_BUDGET_ID = {budget.Id} AND " +
               $"BDG_TXN_STATUS <> 'X' " +
               $"ORDER BY BDG_TXN_NUMBER";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetTransaction>(op);
    }


    static internal List<BudgetEntry> GetTransactionEntries(BudgetTransaction transaction) {
      var sql = "SELECT * FROM FMS_BUDGET_ENTRIES " +
               $"WHERE BDG_ENTRY_TXN_ID = {transaction.Id} AND " +
                     $"BDG_ENTRY_STATUS <> 'X' " +
               $"ORDER BY BDG_ENTRY_POSITION, BDG_ENTRY_YEAR, BDG_ENTRY_MONTH, BDG_ENTRY_DAY, " +
                         $"BDG_ENTRY_CONTROL_NO, BDG_ENTRY_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<BudgetEntry>(op);
    }


    static internal FixedList<BudgetTransaction> SearchTransactions(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM VW_FMS_BUDGET_TRANSACTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetTransaction>(op);
    }


    static internal void WriteEntry(BudgetEntry o) {
      var op = DataOperation.Parse("write_FMS_Budget_Entry",
        o.Id, o.UID, o.Transaction.Id, o.BudgetEntryTypeId, o.Budget.Id,
        o.BudgetAccount.Id, o.BudgetProgram.Id, o.ControlNo, o.Product.Id,
        o.ProductCode, o.ProductName, o.ProductUnit.Id, o.ProductQty,
        o.Project.Id, o.Party.Id, o.EntityTypeId, o.EntityId, o.OperationNo,
        o.Year, o.Month, o.Day, o.BalanceColumn.Id, o.Currency.Id, o.CurrencyAmount,
        o.Deposit, o.Withdrawal, o.ExchangeRate, o.IsAdjustment ? 1 : 0,
        o.Description, o.Justification, o.Identificators, o.Tags,
        o.ExtensionData.ToString(), o.Keywords, o.RelatedEntryId,
        o.Position, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteReopenedEntry(BudgetEntry o, decimal originalAmount) {

      var sql = "UPDATE FMS_BUDGET_ENTRIES " +
                   $"SET BDG_ENTRY_MONTH = {o.Month}, " +
                   $"BDG_ENTRY_PRODUCT_QTY = {o.ProductQty}, " +
                   $"BDG_ENTRY_REQUESTED_AMOUNT = {o.CurrencyAmount}, " +
                   $"BDG_ENTRY_DEPOSIT_AMOUNT = {o.Deposit}, " +
                   $"BDG_ENTRY_WITHDRAWAL_AMOUNT = {o.Withdrawal} " +
               $"WHERE BDG_ENTRY_ID = {o.Id} AND BDG_ENTRY_UID = '{o.UID}'";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);

      if (o.Deposit == 0 || o.IsAdjustment) {
        return;
      }

      sql = "UPDATE OMS_ORDER_ITEMS " +
                $"SET ORDER_ITEM_REQUESTED_QTY = {o.Deposit} " +
            $"WHERE ORDER_ITEM_ID = {o.EntityId} AND ORDER_ITEM_REQUESTED_QTY = {originalAmount}";

      op = DataOperation.Parse(sql);

      DataWriter.Execute(op);

      sql = "UPDATE OMS_ORDER_ITEMS " +
              $"SET ORDER_ITEM_MIN_QTY = {o.Deposit} " +
            $"WHERE ORDER_ITEM_ID = {o.EntityId} AND ORDER_ITEM_MIN_QTY = {originalAmount}";

      op = DataOperation.Parse(sql);

      DataWriter.Execute(op);

      sql = "UPDATE OMS_ORDER_ITEMS " +
              $"SET ORDER_ITEM_MAX_QTY = {o.Deposit} " +
            $"WHERE ORDER_ITEM_ID = {o.EntityId} AND ORDER_ITEM_MAX_QTY = {originalAmount}";

      op = DataOperation.Parse(sql);

      DataWriter.Execute(op);

      sql = "UPDATE OMS_ORDER_ITEMS " +
            $"SET ORDER_ITEM_QTY = {o.Deposit} " +
            $"WHERE ORDER_ITEM_ID = {o.EntityId} AND ORDER_ITEM_QTY = {originalAmount}";

      op = DataOperation.Parse(sql);

      DataWriter.Execute(op);

      sql = "UPDATE OMS_ORDER_ITEMS " +
              $"SET ORDER_ITEM_UNIT_PRICE = {o.Deposit} " +
            $"WHERE ORDER_ITEM_ID = {o.EntityId} AND ORDER_ITEM_UNIT_PRICE = {originalAmount}";

      op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal void WriteTransaction(BudgetTransaction o) {
      var op = DataOperation.Parse("write_FMS_Budget_Transaction",
          o.Id, o.UID, o.TransactionType.Id, o.OperationSource.Id, o.BaseBudget.Id,
          o.BaseParty.Id, o.TransactionNo, o.Description, o.Justification,
          EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags), -1,
          o.HasEntity ? o.GetEntity().Data.BudgetableType.Id : -1,
          o.HasEntity ? o.GetEntity().Id : -1,
          o.PayableId, o.ApplicationDate, o.AppliedBy.Id, o.RecordingDate, o.RecordedBy.Id,
          o.AuthorizationDate, o.AuthorizedBy.Id, o.RequestedDate, o.RequestedBy.Id,
          o.ExtensionData.ToString(), o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class BudgetTransactionDataService

}  // namespace Empiria.Budgeting.Transactions.Data
