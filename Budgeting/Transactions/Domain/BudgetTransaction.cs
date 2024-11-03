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

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;

using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Partitioned type that represents a budget transaction with its entries.</summary>
  [PartitionedType(typeof(BudgetTransactionType))]
  public class BudgetTransaction : BaseObject {

    #region Constructors and parsers

    protected BudgetTransaction(BudgetTransactionType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public BudgetTransaction Parse(int id) => ParseId<BudgetTransaction>(id);

    static public BudgetTransaction Parse(string uid) => ParseKey<BudgetTransaction>(uid);

    static public BudgetTransaction Empty => ParseEmpty<BudgetTransaction>();

    #endregion Constructors and parsers

    #region Properties

    public BudgetTransactionType BudgetTransactionType {
      get {
        return (BudgetTransactionType) base.GetEmpiriaType();
      }
    }


    [DataField("BDG_TXN_SOURCE_ID")]
    public OperationSource OperationSource {
      get;
      private set;
    }


    [DataField("BDG_TXN_BASE_BUDGET_ID")]
    public Budget BaseBudget {
      get;
      private set;
    }


    [DataField("BDG_TXN_BASE_PARTY_ID")]
    public Party BaseParty {
      get;
      private set;
    }


    [DataField("BDG_TXN_NUMBER")]
    public string TransactionNo {
      get;
      private set;
    }


    [DataField("BDG_TXN_DESCRIPTION")]
    public string Description {
      get;
      private set;
    }


    [DataField("BDG_TXN_IDENTIFICATORS")]
    public string Identificators {
      get;
      private set;
    }


    [DataField("BDG_TXN_TAGS")]
    public string Tags {
      get;
      private set;
    }


    [DataField("BDG_TXN_BASE_ENTITY_TYPE_ID")]
    public int BaseEntityTypeId {
      get;
      private set;
    }


    [DataField("BDG_TXN_BASE_ENTITY_ID")]
    public int BaseEntityId {
      get;
      private set;
    }


    [DataField("BDG_TXN_APPLICATION_DATE")]
    public DateTime ApplicationDate {
      get;
      private set;
    }


    [DataField("BDG_TXN_APPLIED_BY_ID")]
    public Party AppliedBy {
      get;
      private set;
    }


    [DataField("BDG_TXN_RECORDING_DATE")]
    public DateTime RecordingDate {
      get;
      private set;
    }


    [DataField("BDG_TXN_RECORDED_BY_ID")]
    public Party RecordedBy {
      get;
      private set;
    }


    [DataField("BDG_TXN_AUTHORIZATION_TIME")]
    public DateTime AuthorizationTime {
      get;
      private set;
    }


    [DataField("BDG_TXN_AUTHORIZED_BY_ID")]
    public Party AuthorizedBy {
      get;
      private set;
    }


    [DataField("BDG_TXN_REQUESTED_TIME")]
    public DateTime RequestedTime {
      get;
      private set;
    }


    [DataField("BDG_TXN_REQUESTED_BY_ID")]
    public Party RequestedBy {
      get;
      private set;
    }


    [DataField("BDG_TXN_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("BDG_TXN_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("BDG_TXN_POSTED_BY_ID")]
    public Party PostedBy {
      get;
      private set;
    }


    [DataField("BDG_TXN_STATUS", Default = BudgetTransactionStatus.Pending)]
    public BudgetTransactionStatus Status {
      get;
      private set;
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
        return BudgetTransactionDataService.GetTransactionEntries(this);
      }
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      BudgetTransactionDataService.WriteTransaction(this);
    }

    #endregion Methods

  }  // class BudgetTransaction

}  // namespace Empiria.Budgeting.Transactions
