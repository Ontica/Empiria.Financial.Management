/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Query DTO                               *
*  Type     : BudgetTransactionsQuery                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query DTO used to retrieve budget transactions.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Enumerates the different workflow stages for a budget transaction.</summary>
  public enum BudgetTransactionStage {

    MyInbox,

    Pending,

    ControlDesk,

    Completed,

    All

  }  // enum BudgetTransactionStage



  /// <summary>Enumerates the different parties that participates in a budget transaction.</summary>
  public enum BudgetTransactionPartyType {

    RequestedBy,

    RegisteredBy,

    AuthorizedBy,

    CompletedBy,

    None,

  }


  /// <summary>Enumerates the different dates for query budget transactions.</summary>
  public enum BudgetTransactionQueryDateType {

    Requested,

    Registered,

    Authorizated,

    Completed,

    None

  }


  /// <summary>Input query DTO used to retrieve budget transactions.</summary>
  public class BudgetTransactionsQuery {

    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string BaseOrgUnitUID {
      get; set;
    } = string.Empty;


    public string OperationSourceUID {
      get; set;
    } = string.Empty;


    public string[] TransactionsID {
      get; set;
    } = new string[0];


    public string Keywords {
      get; set;
    } = string.Empty;


    public string EntriesKeywords {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public BudgetTransactionQueryDateType TransactionDateType {
      get; set;
    } = BudgetTransactionQueryDateType.None;


    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public BudgetTransactionPartyType transactionPartyType {
      get; set;
    } = BudgetTransactionPartyType.None;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public BudgetTransactionStatus Status {
      get; set;
    } = BudgetTransactionStatus.All;


    public BudgetTransactionStage Stage {
      get; set;
    } = BudgetTransactionStage.All;

  }  // class BudgetTransactionsQuery

}  // namespace Empiria.Budgeting.Adapters
