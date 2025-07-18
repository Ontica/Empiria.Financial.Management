﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Information Holder                      *
*  Type     : BudgetEntry                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : An entry in a budget transaction.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions {

  /// <summary>An entry in a budget transaction.</summary>
  public class BudgetEntry : BaseObject {

    #region Constructors and parsers

    private BudgetEntry() {
      // Required by Empiria Framework.
    }

    internal BudgetEntry(BudgetTransaction transaction, int year, int month) {
      Assertion.Require(transaction, nameof(transaction));

      this.Transaction = transaction;
      this.Budget = transaction.BaseBudget;

      this.Year = year;
      this.Month = month;
    }

    static public BudgetEntry Parse(int id) => ParseId<BudgetEntry>(id);

    static public BudgetEntry Parse(string uid) => ParseKey<BudgetEntry>(uid);

    static public BudgetEntry Empty => ParseEmpty<BudgetEntry>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("BDG_ENTRY_TXN_ID")]
    public BudgetTransaction Transaction {
      get; private set;
    }


    [DataField("BDG_ENTRY_TYPE_ID")]
    public int BudgetEntryTypeId {
      get; private set;
    }


    [DataField("BDG_ENTRY_BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }


    [DataField("BDG_ENTRY_BUDGET_ACCT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("BDG_ENTRY_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("BDG_ENTRY_PRODUCT_UNIT_ID")]
    public ProductUnit ProductUnit {
      get; private set;
    }


    [DataField("BDG_ENTRY_PRODUCT_QTY")]
    public decimal ProductQty {
      get; private set;
    }


    [DataField("BDG_ENTRY_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("BDG_ENTRY_PARTY_ID")]
    public Party Party {
      get; private set;
    }


    [DataField("BDG_ENTRY_OPERATION_TYPE_ID")]
    public int OperationTypeId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_OPERATION_ID")]
    public int OperationId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_BASE_ENTITY_ITEM_ID")]
    public int BaseEntityItemId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_YEAR")]
    public int Year {
      get; private set;
    }


    [DataField("BDG_ENTRY_MONTH")]
    public int Month {
      get; private set;
    }


    public string MonthName {
      get {
        return new DateTime(Year, Month, 1).ToString("MMMM");
      }
    }


    [DataField("BDG_ENTRY_DAY")]
    public int Day {
      get; private set;
    }


    [DataField("BDG_ENTRY_BALANCE_COLUMN_ID")]
    public BalanceColumn BalanceColumn {
      get; private set;
    }


    [DataField("BDG_ENTRY_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("BDG_ENTRY_ORIGINAL_AMOUNT")]
    public decimal OriginalAmount {
      get; private set;
    }


    public decimal Amount {
      get {
        return Deposit + Withdrawal;
      }
    }


    [DataField("BDG_ENTRY_DEPOSIT_AMOUNT")]
    public decimal Deposit {
      get; private set;
    }


    [DataField("BDG_ENTRY_WITHDRAWAL_AMOUNT")]
    public decimal Withdrawal {
      get; private set;
    }


    [DataField("BDG_ENTRY_EXCHANGE_RATE")]
    public decimal ExchangeRate {
      get; private set;
    }


    [DataField("BDG_ENTRY_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("BDG_ENTRY_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("BDG_ENTRY_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("BDG_ENTRY_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("BDG_ENTRY_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get; private set;
    }


    [DataField("BDG_ENTRY_LINKED_BDG_ENTRY_ID")]
    internal int LinkedBudgetEntryId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_POSITION")]
    public int Position {
      get; private set;
    }


    [DataField("BDG_ENTRY_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BDG_ENTRY_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BDG_ENTRY_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BudgetAccount.Keywords, Transaction.Keywords);
      }
    }


    #endregion Properties

    #region Methods

    internal void Delete() {
      Status = TransactionStatus.Deleted;

      MarkAsDirty();
    }

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      BudgetTransactionDataService.WriteEntry(this);
    }


    internal void Update(BudgetEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      this.BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, BudgetAccount);
      this.Product = Patcher.Patch(fields.ProductUID, Product.Empty);
      this.ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit.Empty);
      this.ProductQty = fields.ProductQty;
      this.Project = Patcher.Patch(fields.ProjectUID, Project.Empty);
      this.Party = Patcher.Patch(fields.PartyUID, Party.Empty);
      this.OperationTypeId = fields.OperationTypeId;
      this.OperationId = fields.OperationId;
      this.BaseEntityItemId = fields.BaseEntityItemId;
      this.BalanceColumn = Patcher.Patch(fields.BalanceColumnUID, BalanceColumn);
      this.Description = EmpiriaString.Clean(fields.Description);
      this.Justification = EmpiriaString.Clean(fields.Justification);
      this.Year = fields.Year != 0 ? fields.Year : this.Year;
      this.Month = fields.Month != 0 ? fields.Month : this.Month;
      this.Currency = Patcher.Patch(fields.CurrencyUID, Budget.BudgetType.Currency);
      this.OriginalAmount = fields.OriginalAmount != 0 ? fields.OriginalAmount : Math.Abs(fields.Amount);
      this.Deposit = fields.Amount > 0 ? fields.Amount : 0m;
      this.Withdrawal = fields.Amount < 0 ? Math.Abs(fields.Amount) : 0m;
      this.ExchangeRate = fields.ExchangeRate;

      MarkAsDirty();
    }

    #endregion Methods

  }  // class BudgetEntry

}  // namespace Empiria.Budgeting.Transactions
