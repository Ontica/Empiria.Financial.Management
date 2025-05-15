/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetTransactionDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used for budget transactions.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Budgeting.Transactions.Adapters {


  /// <summary>Output holder DTO used for a budget transaction.</summary>
  public class BudgetTransactionHolderDto {

    public BudgetTransactionDto Transaction {
      get; internal set;
    }

    public FixedList<BudgetEntryDescriptorDto> Entries {
      get; internal set;
    }

    public BudgetEntriesByYearTableDto GroupedEntries {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }


    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BudgetTransactionActions Actions {
      get; internal set;
    }

  }  // class BudgetTransactionHolderDto



  /// <summary>Action flags for budget transactions.</summary>
  public class BudgetTransactionActions : BaseActions {

    public bool CanAuthorize {
      get; internal set;
    }

    public bool CanClose {
      get; internal set;
    }

    public bool CanReject {
      get; internal set;
    }

    public bool CanSendToAuthorization {
      get; internal set;
    }

  }  // class BudgetTransactionActions



  /// <summary>Output DTO used for budget transactions.</summary>
  public class BudgetTransactionDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto BudgetType {
      get; internal set;
    }

    public TransactionTypeForEditionDto TransactionType {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public NamedEntityDto Budget {
      get; internal set;
    }

    public NamedEntityDto BaseParty {
      get; internal set;
    }

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public NamedEntityDto BaseEntityType {
      get; internal set;
    }

    public NamedEntityDto BaseEntity {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public DateTime RecordingDate {
      get; internal set;
    }

    public NamedEntityDto RecordedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public NamedEntityDto RequestedBy {
      get; internal set;
    }

    public DateTime AuthorizationDate {
      get; internal set;
    }

    public NamedEntityDto AuthorizedBy {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public NamedEntityDto AppliedBy {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // BudgetTransactionDto



  /// <summary>Output DTO used to display budget transactions in lists.</summary>
  public class BudgetTransactionDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string BudgetTypeName {
      get; internal set;
    }

    public string TransactionTypeName {
      get; internal set;
    }

    public string TransactionNo {
      get; internal set;
    }

    public string BudgetName {
      get; internal set;
    }

    public string BasePartyName {
      get; internal set;
    }

    public string OperationSourceName {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime RecordingDate {
      get; internal set;
    }

    public string RecordedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public string RequestedBy {
      get; internal set;
    }

    public DateTime AuthorizationDate {
      get; internal set;
    }

    public string AuthorizedBy {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public string AppliedBy {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // BudgetTransactionDescriptorDto

}  // namespace Empiria.Budgeting.Transactions.Adapters
