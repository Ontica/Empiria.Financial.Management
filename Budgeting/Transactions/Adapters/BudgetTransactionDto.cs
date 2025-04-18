﻿/* Empiria Financial *****************************************************************************************
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

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }


    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public BudgetTransactionActions Actions {
      get; internal set;
    }

  }  // class BudgetTransactionHolderDto



  public class BudgetTransactionActions : BaseActions {

    public bool CanAuthorize {
      get; internal set;
    }

    public bool CanReject {
      get; internal set;
    }

  }


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

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime ApplicationDate {
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

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime ApplicationDate {
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
