/* Empiria Financial *****************************************************************************************
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
using Empiria.Locations;
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
      Assertion.Require(year >= transaction.BaseBudget.Year,
                       "Year must be greater than or equal to the base budget year.");
      Assertion.Require(0 <= month && month <= 12,
                       "Month must be between 0 and 12 (0 means the whole year).");

      Transaction = transaction;
      Budget = transaction.BaseBudget;

      Year = year;
      Month = month;
    }


    internal BudgetEntry(BudgetTransaction transaction, BudgetableItemData entry,
                         BalanceColumn balanceColumn, bool isDeposit) {
      Assertion.Require(transaction, nameof(transaction));
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(balanceColumn, nameof(balanceColumn));

      Transaction = transaction;
      Budget = (Budget) entry.Budget;
      BudgetAccount = (BudgetAccount) entry.BudgetAccount;
      BudgetProgram = BudgetAccount.BudgetProgram;
      BalanceColumn = balanceColumn;

      Year = entry.BudgetingDate.Year;
      Month = entry.BudgetingDate.Month;
      Day = entry.BudgetingDate.Day;

      Product = entry.Product;
      ProductCode = entry.ProductCode;
      ProductName = entry.ProductName;
      ProductUnit = entry.ProductUnit;
      ProductQty = entry.ProductQty;
      OriginCountry = entry.OriginCountry;
      Project = entry.Project;
      Party = entry.Party;
      EntityTypeId = entry.BudgetableItem.GetEmpiriaType().Id;
      EntityId = entry.BudgetableItem.Id;

      Currency = entry.Currency;
      ExchangeRate = entry.ExchangeRate;
      CurrencyAmount = entry.CurrencyAmount;
      Deposit = isDeposit ? entry.Amount : 0m;
      Withdrawal = isDeposit ? 0m : entry.Amount;

      Description = entry.Description;
      Justification = entry.Justification;

      RelatedEntryId = entry.HasRelatedBudgetEntry ? entry.RelatedBudgetEntry.Id : -1;
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


    [DataField("BDG_ENTRY_PROGRAM_ID")]
    public BudgetProgram BudgetProgram {
      get; private set;
    }


    [DataField("BDG_ENTRY_CONTROL_NO")]
    public string ControlNo {
      get; internal set;
    }


    [DataField("BDG_ENTRY_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("BDG_ENTRY_PRODUCT_CODE")]
    public string ProductCode {
      get; private set;
    }


    [DataField("BDG_ENTRY_PRODUCT_NAME")]
    public string ProductName {
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


    public Country OriginCountry {
      get {
        return ExtensionData.Get("orderItem/originCountryId", Country.Default);
      }
      private set {
        ExtensionData.SetIf("orderItem/originCountryId", value, value.Distinct(Country.Default));
      }
    }


    [DataField("BDG_ENTRY_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("BDG_ENTRY_PARTY_ID")]
    public Party Party {
      get; private set;
    }


    [DataField("BDG_ENTRY_ENTITY_TYPE_ID")]
    public int EntityTypeId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_ENTITY_ID")]
    public int EntityId {
      get; private set;
    } = -1;


    [DataField("BDG_ENTRY_OPERATION_NO")]
    public string OperationNo {
      get; private set;
    }


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
        if (Month == 0) {
          return $"Durante {Year}";
        } else {
          return new DateTime(Year, Month, 1).ToString("MMMM");
        }
      }
    }


    [DataField("BDG_ENTRY_DAY")]
    public int Day {
      get; private set;
    }


    public DateTime Date {
      get {
        return new DateTime(Year, Month == 0 ? 1 : Month, Day == 0 ? 1 : Day);
      }
    }


    [DataField("BDG_ENTRY_BALANCE_COLUMN_ID")]
    public BalanceColumn BalanceColumn {
      get; private set;
    }


    [DataField("BDG_ENTRY_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("BDG_ENTRY_REQUESTED_AMOUNT")]
    public decimal CurrencyAmount {
      get; private set;
    }


    [DataField("BDG_ENTRY_DEPOSIT_AMOUNT")]
    public decimal Deposit {
      get; private set;
    }


    [DataField("BDG_ENTRY_WITHDRAWAL_AMOUNT")]
    public decimal Withdrawal {
      get; private set;
    }

    public decimal Amount {
      get {
        return Deposit + Withdrawal;
      }
    }

    [DataField("BDG_ENTRY_EXCHANGE_RATE")]
    public decimal ExchangeRate {
      get; private set;
    }


    [DataField("BDG_ENTRY_IS_ADJUSTMENT", ConvertFrom = typeof(int))]
    public bool IsAdjustment {
      get; private set;
    }


    public bool NotAdjustment {
      get {
        return !IsAdjustment;
      }
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


    [DataField("BDG_ENTRY_RELATED_ENTRY_ID")]
    internal int RelatedEntryId {
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

    public bool NoRejected {
      get {
        return Status != TransactionStatus.Rejected &&
               Status != TransactionStatus.Deleted;
      }
    }

    #endregion Properties

    #region Methods

    internal BudgetEntry CloneFor(BudgetTransaction transaction, DateTime date,
                                  BalanceColumn balanceColumn, bool deposit, bool isAdjustment = false) {
      var budgetEntry = new BudgetEntry(transaction, date.Year, date.Month) {
        BudgetAccount = this.BudgetAccount,
        BudgetProgram = this.BudgetProgram,
        Product = this.Product,
        ProductCode = this.ProductCode,
        ProductName = this.ProductName,
        ProductUnit = this.ProductUnit,
        ProductQty = this.ProductQty,
        OriginCountry = this.OriginCountry,
        Project = this.Project,
        Party = this.Party,
        EntityTypeId = this.EntityTypeId,
        EntityId = this.EntityId,
        OperationNo = this.OperationNo,
        Day = date.Day,
        BalanceColumn = balanceColumn,
        Currency = this.Currency,
        ExchangeRate = this.ExchangeRate,
        CurrencyAmount = this.CurrencyAmount,
        Withdrawal = deposit ? 0m : this.Amount,
        Deposit = deposit ? this.Amount : 0m,
        IsAdjustment = isAdjustment,
        Description = this.Description,
        Justification = this.Justification,
        Identificators = this.Identificators,
        Tags = this.Tags,
        ControlNo = this.ControlNo,
        RelatedEntryId = this.RelatedEntryId
      };

      return budgetEntry;
    }


    internal void Close() {
      Status = TransactionStatus.Closed;

      MarkAsDirty();
    }


    internal void Delete() {
      Status = TransactionStatus.Deleted;

      MarkAsDirty();
    }


    internal void Reject() {
      Status = TransactionStatus.Rejected;

      MarkAsDirty();
    }


    internal void SetControlNo(string controlNo) {
      Assertion.Require(controlNo, nameof(controlNo));

      ControlNo = controlNo;

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

      Budget = Patcher.Patch(fields.BudgetUID, Transaction.BaseBudget);
      BudgetAccount = Patcher.Patch(fields.BudgetAccountUID, BudgetAccount);

      BudgetProgram = BudgetAccount.BudgetProgram;

      Product = Patcher.Patch(fields.ProductUID, Product.Empty);
      ProductCode = EmpiriaString.Clean(fields.ProductCode);

      ProductName = EmpiriaString.Clean(fields.ProductName);
      ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit.Empty);
      ProductQty = fields.ProductQty;
      OriginCountry = Patcher.Patch(fields.OriginCountryUID, Country.Default);
      Project = Patcher.Patch(fields.ProjectUID, Project.Empty);
      Party = Patcher.Patch(fields.PartyUID, Party.Empty);
      EntityTypeId = fields.EntityTypeId;
      EntityId = fields.EntityId;
      OperationNo = fields.OperationNo;
      BalanceColumn = Patcher.Patch(fields.BalanceColumnUID, BalanceColumn);
      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      Year = Budget.Year;
      Month = fields.Month;
      Day = fields.Day;
      Currency = Patcher.Patch(fields.CurrencyUID, Budget.BudgetType.Currency);
      ExchangeRate = fields.ExchangeRate;
      CurrencyAmount = fields.CurrencyAmount != 0 ? fields.CurrencyAmount : Math.Abs(fields.Amount);
      Deposit = fields.Amount > 0 ? fields.Amount : 0m;
      Withdrawal = fields.Amount < 0 ? Math.Abs(fields.Amount) : 0m;

      if (fields.RelatedEntryUID.Length != 0) {
        RelatedEntryId = Parse(fields.RelatedEntryUID).Id;
      }

      MarkAsDirty();
    }


    #endregion Methods

  }  // class BudgetEntry

}  // namespace Empiria.Budgeting.Transactions
