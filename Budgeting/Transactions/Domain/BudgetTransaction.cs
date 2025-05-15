﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Partitioned Aggregate Root              *
*  Type     : BudgetTransaction                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a budget transaction with its entries.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;

using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Partitioned type that represents a budget transaction with its entries.</summary>
  [PartitionedType(typeof(BudgetTransactionType))]
  public class BudgetTransaction : BaseObject {

    #region Fields

    private readonly string TO_ASSIGN_TRANSACTION_NO = "Por asignar";

    private Lazy<List<BudgetEntry>> _entries = new Lazy<List<BudgetEntry>>();
    private List<BudgetEntry> _deletedEntries = new List<BudgetEntry>();

    #endregion Fields

    #region Constructors and parsers

    protected BudgetTransaction(BudgetTransactionType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal protected BudgetTransaction(BudgetTransactionType transactionType,
                                         Budget baseBudget) : base(transactionType) {
      Assertion.Require(baseBudget, nameof(baseBudget));

      this.BaseBudget = baseBudget;
      this.EntityTypeId = -1;
      this.EntityId = -1;
    }

    internal protected BudgetTransaction(BudgetTransactionType transactionType,
                                         Budget baseBudget,
                                         BaseObject entity) : base(transactionType) {
      Assertion.Require(baseBudget, nameof(baseBudget));

      this.BaseBudget = baseBudget;
      this.EntityTypeId = entity.GetEmpiriaType().Id;
      this.EntityId = entity.Id;
    }

    static public BudgetTransaction Parse(int id) => ParseId<BudgetTransaction>(id);

    static public BudgetTransaction Parse(string uid) => ParseKey<BudgetTransaction>(uid);

    static public FixedList<BudgetTransaction> GetFor(IPayableEntity payableEntity) {
      return BudgetTransactionDataService.GetTransactions(payableEntity);
    }

    static public BudgetTransaction Empty => ParseEmpty<BudgetTransaction>();

    protected override void OnLoad() {
      Reload();
    }

    #endregion Constructors and parsers

    #region Properties

    public BudgetTransactionType BudgetTransactionType {
      get {
        return (BudgetTransactionType) base.GetEmpiriaType();
      }
    }


    [DataField("BDG_TXN_BASE_BUDGET_ID")]
    public Budget BaseBudget {
      get; private set;
    }


    [DataField("BDG_TXN_BASE_PARTY_ID")]
    public Party BaseParty {
      get; private set;
    }


    [DataField("BDG_TXN_NUMBER")]
    public string TransactionNo {
      get; private set;
    }


    [DataField("BDG_TXN_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("BDG_TXN_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("BDG_TXN_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("BDG_TXN_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("BDG_TXN_ENTITY_TYPE_ID")]
    private int EntityTypeId {
      get; set;
    }


    [DataField("BDG_TXN_ENTITY_ID")]
    private int EntityId {
      get; set;
    }


    public bool HasEntity {
      get {
        return EntityTypeId != -1;
      }
    }


    public BaseObject GetEntity() {
      return BaseObject.Parse(EntityTypeId, EntityId);
    }


    [DataField("BDG_TXN_CONTRACT_ID")]
    internal int ContractId {
      get; private set;
    }


    [DataField("BDG_TXN_PAYABLE_ID")]
    internal int PayableId {
      get; private set;
    }


    [DataField("BDG_TXN_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get; private set;
    }


    [DataField("BDG_TXN_APPLIED_BY_ID")]
    public Party AppliedBy {
      get; private set;
    }


    [DataField("BDG_TXN_RECORDING_DATE")]
    public DateTime RecordingDate {
      get; private set;
    }


    [DataField("BDG_TXN_RECORDED_BY_ID")]
    public Party RecordedBy {
      get; private set;
    }


    [DataField("BDG_TXN_AUTHORIZATION_TIME")]
    public DateTime AuthorizationDate {
      get; private set;
    }


    [DataField("BDG_TXN_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("BDG_TXN_REQUESTED_TIME")]
    public DateTime RequestedDate {
      get; private set;
    }


    [DataField("BDG_TXN_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get; private set;
    }


    [DataField("BDG_TXN_SOURCE_ID")]
    public OperationSource OperationSource {
      get; private set;
    }


    [DataField("BDG_TXN_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get; private set;
    }


    [DataField("BDG_TXN_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BDG_TXN_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BDG_TXN_STATUS", Default = BudgetTransactionStatus.Pending)]
    public BudgetTransactionStatus Status {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(TransactionNo, Description, Identificators, Tags,
                                           BudgetTransactionType.DisplayName,
                                           BaseBudget.Keywords, BaseParty.Keywords);
      }
    }


    public FixedList<BudgetEntry> Entries {
      get {
        return _entries.Value.ToFixedList();
      }
    }


    public bool HasTransactionNo {
      get {
        return this.TransactionNo.Length != 0 &&
               this.TransactionNo != TO_ASSIGN_TRANSACTION_NO;
      }
    }

    internal BudgetTransactionRules Rules {
      get {
        return new BudgetTransactionRules(this);
      }
    }

    #endregion Properties

    #region Methods

    internal BudgetEntry AddEntry(BudgetEntryFields entryFields) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");

      Assertion.Require(entryFields, nameof(entryFields));

      if (TryGetEntry(entryFields) != null) {
        Assertion.RequireFail("Ya existe un movimiento con la misma información para el " +
                              "mismo mes y año en esta transacción presupuestal.");
      }

      var entry = new BudgetEntry(this, entryFields.Year, entryFields.Month);

      entry.Update(entryFields);

      _entries.Value.Add(entry);

      return entry;
    }


    internal void Authorize() {
      Assertion.Require(Rules.CanAuthorize, "Current user can not authorize this transaction.");

      if (!HasTransactionNo) {
        TransactionNo = BudgetTransactionDataService.GetNextTransactionNo(this);
      }

      this.AuthorizedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.AuthorizationDate = DateTime.Now;
      this.Status = BudgetTransactionStatus.Authorized;
    }


    internal void Close() {
      Assertion.Require(Rules.CanClose, "Current user can not close this transaction.");

      this.AppliedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      if (ApplicationDate == ExecutionServer.DateMaxValue) {
        ApplicationDate = DateTime.Now;
      }
      this.Status = BudgetTransactionStatus.Closed;
    }


    internal void DeleteOrCancel() {
      Assertion.Require(Rules.CanDelete, "Current user can not delete or cancel this transaction.");

      Assertion.Require(this.Status == BudgetTransactionStatus.Pending,
                       $"Can not delete or cancel budget transaction. Its status is {Status.GetName()}.");

      if (HasTransactionNo) {
        this.Status = BudgetTransactionStatus.Canceled;
      } else {
        this.TransactionNo = "Eliminada";
        this.Status = BudgetTransactionStatus.Deleted;
      }
    }


    internal BudgetEntry GetEntry(string budgetEntryUID) {
      Assertion.Require(budgetEntryUID, nameof(budgetEntryUID));

      BudgetEntry entry = _entries.Value.Find(x => x.UID == budgetEntryUID);

      Assertion.Require(entry, $"Budget entry with UID '{budgetEntryUID}' not found.");

      return entry;
    }


    [DataField("BDG_TXN_TOTAL")]
    private decimal _total = 0;

    public decimal GetTotal() {
      if (_entries.IsValueCreated) {
        return _entries.Value.Sum(x => x.Deposit);
      } else {
        return _total;
      }
    }


    protected override void OnSave() {
      if (IsNew) {
        TransactionNo = TO_ASSIGN_TRANSACTION_NO;
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      } else if (Status == BudgetTransactionStatus.Pending) {
        RecordedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        RecordingDate = DateTime.Now;
      }

      BudgetTransactionDataService.WriteTransaction(this);

      foreach (var entry in _entries.Value) {
        entry.Save();
      }
      foreach (var deletedEntry in _deletedEntries) {
        deletedEntry.Save();
      }
      _deletedEntries.Clear();
    }


    internal void Reject() {
      Assertion.Require(Rules.CanReject,
                        $"Can not reject this budget transaction. Its status is {Status.GetName()}.");

      this.RequestedBy = Party.Empty;
      this.RequestedDate = ExecutionServer.DateMaxValue;

      this.AuthorizedBy = Party.Empty;
      this.AuthorizationDate = ExecutionServer.DateMaxValue;

      this.Status = BudgetTransactionStatus.Pending;
    }


    internal void SendToAuthorization() {
      Assertion.Require(Rules.CanSendToAuthorization,
                        "Current user can not send this transaction to authorization.");

      if (!HasTransactionNo) {
        TransactionNo = BudgetTransactionDataService.GetNextTransactionNo(this);
      }

      this.RequestedBy = PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.RequestedDate = DateTime.Now;
      this.Status = BudgetTransactionStatus.OnAuthorization;
    }


    internal void RemoveEntry(BudgetEntry entry) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(_entries.Value.Contains(entry),
                        "Entry to remove does not belong to this transaction.");

      entry.Delete();

      _deletedEntries.Add(entry);
      _entries.Value.Remove(entry);
    }


    internal BudgetEntry TryGetEntry(BudgetEntryFields fields) {
      var column = FieldPatcher.PatchField(fields.BalanceColumnUID, BalanceColumn.Empty);
      var account = FieldPatcher.PatchField(fields.BudgetAccountUID, BudgetAccount.Empty);
      var product = FieldPatcher.PatchField(fields.ProductUID, Product.Empty);
      var productUnit = FieldPatcher.PatchField(fields.ProductUnitUID, ProductUnit.Empty);
      var project = FieldPatcher.PatchField(fields.ProjectUID, Project.Empty);
      var currency = FieldPatcher.PatchField(fields.CurrencyUID, BaseBudget.BudgetType.Currency);

      return _entries.Value.Find(x => x.BalanceColumn.Equals(column) &&
                                      x.BudgetAccount.Equals(account) &&
                                      x.Product.Equals(product) &&
                                      x.ProductUnit.Equals(productUnit) &&
                                      x.Project.Equals(project) &&
                                      x.Currency.Equals(currency) &&
                                      x.Year == fields.Year &&
                                      x.Month == fields.Month);
    }


    internal void Update(BudgetTransactionFields fields) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      BaseParty = PatchField(fields.BasePartyUID, BaseParty);
      RequestedBy = PatchField(fields.RequestedByUID, RequestedBy);
      ContractId = fields.ContractId;
      PayableId = fields.PayableId;
      OperationSource = PatchField(fields.OperationSourceUID, OperationSource);
      ApplicationDate = PatchField(fields.ApplicationDate, ApplicationDate);
    }


    internal void UpdateEntry(BudgetEntry budgetEntry, BudgetEntryFields fields) {
      var currentEntry = TryGetEntry(fields);

      if (currentEntry != null && !budgetEntry.Equals(currentEntry)) {
        Assertion.RequireFail("Ya existe un movimiento con la misma información para el mismo mes y año " +
                              "en esta transacción presupuestal.");
      }

      budgetEntry.Update(fields);
    }


    internal void UpdateEntries(FixedList<BudgetEntry> entries) {
      Assertion.Require(Rules.CanUpdate, "Current user can not update this transaction.");
      Assertion.Require(entries, nameof(entries));

      Assertion.Require(entries.CountAll(x => x.Transaction.Equals(this)) == entries.Count,
                        "All entries must belong to this transaction.");

      foreach (var entry in entries) {
        if (entry.IsNew) {
          _entries.Value.Add(entry);
        } else if (entry.Status == StateEnums.TransactionStatus.Deleted) {
          _deletedEntries.Add(entry);
          _entries.Value.Remove(entry);
        }
      }
    }

    #endregion Methods

    #region Helpers

    private void Reload() {
      _entries = new Lazy<List<BudgetEntry>>(() => BudgetTransactionDataService.GetTransactionEntries(this));
    }

    #endregion Helpers

  }  // class BudgetTransaction

}  // namespace Empiria.Budgeting.Transactions
