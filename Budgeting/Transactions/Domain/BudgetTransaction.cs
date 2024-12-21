/* Empiria Financial *****************************************************************************************
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

using Empiria.Financial;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Partitioned type that represents a budget transaction with its entries.</summary>
  [PartitionedType(typeof(BudgetTransactionType))]
  public class BudgetTransaction : BaseObject {

    #region Fields

    private Lazy<List<BudgetEntry>> _entries = new Lazy<List<BudgetEntry>>();

    #endregion Fields

    #region Constructors and parsers

    protected BudgetTransaction(BudgetTransactionType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
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


    [DataField("BDG_TXN_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("BDG_TXN_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("BDG_TXN_ENTITY_TYPE_ID")]
    internal int EntityTypeId {
      get; private set;
    }


    [DataField("BDG_TXN_ENTITY_ID")]
    internal int EntityId {
      get; private set;
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
    public DateTime AuthorizationTime {
      get; private set;
    }


    [DataField("BDG_TXN_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get; private set;
    }


    [DataField("BDG_TXN_REQUESTED_TIME")]
    public DateTime RequestedTime {
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


    [DataField("BDG_TXN_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
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

    #endregion Properties

    #region Methods

    internal void AddEntry(BudgetEntryFields entryFields) {
      Assertion.Require(entryFields, nameof(entryFields));

      var entry = new BudgetEntry(this);

      entry.Update(entryFields);

      _entries.Value.Add(entry);
    }


    internal void Authorize() {
      this.AuthorizedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
      this.AuthorizationTime = DateTime.Now;
      this.Status = TransactionStatus.Completed;
    }


    protected override void OnSave() {
      if (IsNew) {
        TransactionNo = BudgetTransactionDataService.GetNextTransactionNo(this);
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }

      BudgetTransactionDataService.WriteTransaction(this);

      foreach (var entry in _entries.Value) {
        entry.Save();
      }
    }


    internal void Reload() {
      _entries = new Lazy<List<BudgetEntry>>(() => BudgetTransactionDataService.GetTransactionEntries(this));
    }


    internal void RemoveEntry(BudgetEntry entry) {
      Assertion.Require(entry, nameof(entry));
      Assertion.Require(_entries.Value.Contains(entry),
                        "Entry to remove does not belong to this transaction.");

      entry.Delete();

      _entries.Value.Remove(entry);
    }


    internal void SendToAuthorization() {
      this.Status = TransactionStatus.OnAuthorization;
    }


    internal void Update(BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      Description = PatchCleanField(fields.Description, Description);
      BaseParty = PatchField(fields.BasePartyUID, BaseParty);
      RequestedBy = PatchField(fields.RequestedByUID, RequestedBy);
      ContractId = fields.ContractId;
      PayableId = fields.PayableId;
      OperationSource = PatchField(fields.OperationSourceUID, OperationSource);
      ApplicationDate = PatchField(fields.ApplicationDate, ApplicationDate);
    }

    #endregion Methods

  }  // class BudgetTransaction

}  // namespace Empiria.Budgeting.Transactions
