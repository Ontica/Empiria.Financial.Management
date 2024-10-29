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
using Empiria.Parties;

namespace Empiria.Budgeting.Transactions {

  /// <summary>An entry in a budget transaction.</summary>
  public class BudgetEntry : BaseObject {

    #region Constructors and parsers

    private BudgetEntry() {
      // Required by Empiria Framework.
    }

    static public BudgetEntry Parse(int id) => ParseId<BudgetEntry>(id);

    static public BudgetEntry Parse(string uid) => ParseKey<BudgetEntry>(uid);

    static public BudgetEntry Empty => ParseEmpty<BudgetEntry>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("BDG_ENTRY_TXN_ID")]
    public BudgetTransaction BudgetTransaction {
      get;
      private set;
    }


    //[DataField("BDG_ENTRY_TYPE_ID")]
    public int BudgetEntryTypeId {
      get {
        return -1;
      }
    }


    [DataField("BDG_ENTRY_BUDGET_ID")]
    public Budget Budget {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_YEAR")]
    public int Year {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_MONTH")]
    public int Month {
      get;
      private set;
    }


    public string MonthName {
      get {
        return new DateTime(Year, Month, 1).ToString("MMMM");
      }
    }


    [DataField("BDG_ENTRY_DAY")]
    public int Day {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_BUDGET_ACCT_ID")]
    public BudgetAccount BudgetAccount {
      get;
      private set;
    }


    // [DataField("BDG_ENTRY_SUBLEDGER_ACCT_ID")]
    public int SubledgerAccountId {
      get {
        return -1;
      }
    }


    // [DataField("BDG_ENTRY_OPERATION_TYPE_ID")]
    public int OperationTypeId {
      get {
        return -1;
      }
    }


    // [DataField("BDG_ENTRY_OPERATION_ID")]
    public int OperationId {
      get {
        return -1;
      }
    }


    [DataField("BDG_ENTRY_BALANCE_COLUMN_ID")]
    public BalanceColumn BalanceColumn {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_CURRENCY_ID")]
    public Currency Currency {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_DEPOSIT_AMOUNT")]
    public decimal Deposit {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_WITHDRAWAL_AMOUNT")]
    public decimal Withdrawal {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_POSTED_BY_ID")]
    public Party PostedBy {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_POSTING_TIME")]
    public DateTime PostingTime {
      get;
      private set;
    }


    [DataField("BDG_ENTRY_STATUS", Default = BudgetTransactionStatus.Pending)]
    public BudgetTransactionStatus Status {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BudgetAccount.Keywords, BudgetTransaction.Keywords);
      }
    }

    #endregion Properties

  }  // class BudgetEntry

}  // namespace Empiria.Budgeting.Transactions
