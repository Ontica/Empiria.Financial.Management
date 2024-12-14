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
  static internal class BudgetTransactionDataService {

    static internal string GetNextTransactionNo(BudgetTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      string transactionPrefix = transaction.BudgetTransactionType.Prefix;
      string budgetPrefix = transaction.BaseBudget.BudgetType.Prefix;

      int year = transaction.BaseBudget.Year;

      string prefix = $"{year}-{transactionPrefix}-{budgetPrefix}";

      string sql = "SELECT MAX(BDG_TXN_NUMBER) " +
                   "FROM FMS_BUDGET_TRANSACTIONS " +
                   $"WHERE BDG_TXN_NUMBER LIKE '{prefix}-%'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), String.Empty);

      if (lastUniqueID != null && lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID.Split('-')[3]) + 1;

        return $"{prefix}-{consecutive:00000}";

      } else {
        return $"{prefix}-00001";
      }
    }


    static internal FixedList<BudgetTransaction> GetTransactions(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      var baseObject = BaseObject.Parse(payableEntity.Type.UID, payableEntity.UID);

      var filter = $"BDG_TXN_BASE_ENTITY_TYPE_ID = {baseObject.GetEmpiriaType().Id} AND " +
                   $"BDG_TXN_BASE_ENTITY_ID = {baseObject.Id} AND " +
                   $"BDG_TXN_STATUS <> 'X'";

      return SearchTransactions(filter, "BDG_TXN_ID");
    }


    static internal List<BudgetEntry> GetTransactionEntries(BudgetTransaction transaction) {
      var sql = "SELECT * FROM FMS_BUDGET_ENTRIES " +
               $"WHERE BDG_ENTRY_TXN_ID = {transaction.Id} AND " +
                     $"BDG_ENTRY_STATUS <> 'X' " +
               $"ORDER BY BGD_ENTRY_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<BudgetEntry>(op);
    }


    static internal FixedList<BudgetTransaction> SearchTransactions(string filter, string sort) {
      Assertion.Require(filter, nameof(filter));
      Assertion.Require(sort, nameof(sort));

      var sql = "SELECT * FROM FMS_BUDGET_TRANSACTIONS " +
               $"WHERE {filter} " +
               $"ORDER BY {sort}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<BudgetTransaction>(op);
    }


    static internal void WriteEntry(BudgetEntry o) {
      var op = DataOperation.Parse("write_FMS_Budget_Entry",
        o.Id, o.UID, o.BudgetTransaction.Id, o.BudgetEntryTypeId, o.Budget.Id,
        o.BudgetAccount.Id, o.Product.Id, o.ProductUnit.Id, o.Project.Id, o.Party.Id,
        o.OperationTypeId, o.OperationId, o.BaseEntityItemId, o.Year, o.Month, o.Day,
        o.BalanceColumn.Id, o.Currency.Id, o.Deposit, o.Withdrawal, o.ExchangeRate,
        o.Description, o.Identificators, o.Tags, o.ExtensionData.ToString(), o.Keywords,
        o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteTransaction(BudgetTransaction o) {
      var op = DataOperation.Parse("write_FMS_Budget_Transaction",
        o.Id, o.UID, o.BudgetTransactionType.Id, o.OperationSource.Id,
        o.BaseBudget.Id, o.BaseParty.Id, o.TransactionNo, o.Description,
        o.Identificators, o.Tags, o.BaseEntityTypeId, o.BaseEntityId,
        o.ApplicationDate, o.AppliedBy.Id, o.RecordingDate, o.RecordedBy.Id,
        o.AuthorizationTime, o.AuthorizedBy.Id, o.RequestedTime, o.RequestedBy.Id,
        o.ExtensionData.ToString(), o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class BudgetTransactionDataService

}  // namespace Empiria.Budgeting.Transactions.Data
