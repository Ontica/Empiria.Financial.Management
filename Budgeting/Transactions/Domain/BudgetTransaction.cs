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

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Parties;

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
    public OperationSource Source {
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


    [DataField("BDG_TXN_RECEIVABLE_ID")]
    public int ReceivableId {
      get;
      private set;
    }


    [DataField("BDG_TXN_PAYABLE_ID")]
    public int PayableId {
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
    protected JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("BDG_TXN_PARENT_ID")]
    private int _parentId;
    public BudgetTransaction Parent {
      get {
        return Parse(_parentId);
      }
      private set {
        _parentId = value.Id;
      }
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
        return EmpiriaString.BuildKeywords(TransactionNo, Description, BudgetTransactionType.DisplayName,
                                           BaseBudget.Keywords, BaseParty.Keywords);
      }
    }

    #endregion Properties

  }  // class BudgetTransaction

}  // namespace Empiria.Budgeting.Transactions
