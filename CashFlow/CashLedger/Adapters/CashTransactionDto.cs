/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashTransactionDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output holder DTO used for a cash transaction.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output holder DTO used for a cash transaction.</summary>
  public class CashTransactionHolderDto {

    public CashTransactionDescriptor Transaction {
      get; set;
    }

    public FixedList<CashTransactionEntryDto> Entries {
      get; set;
    }

    public FixedList<DocumentDto> Documents {
      get; set;
    } = new FixedList<DocumentDto>();


    public FixedList<HistoryEntryDto> History {
      get; set;
    } = new FixedList<HistoryEntryDto>();


    public CashTransactionActions Actions {
      get; set;
    } = new CashTransactionActions { CanUpdate = true };

  }  // class CashTransactionHolderDto



  /// <summary>Action flags for cash transactions.</summary>
  public class CashTransactionActions {

    public bool CanUpdate {
      get; set;
    }

  }  // class CashTransactionActions



  /// <summary>Output DTO used to retrieve cash ledger entries and its transaction.</summary>
  public class CashEntryDescriptor {

    public CashTransactionDescriptor Transaction {
      get; set;
    }

    public CashTransactionEntryDto Entry {
      get; set;
    }

  }  // class CashEntryDescriptor


  /// <summary>Output DTO used to retrieve cash ledger transaction entries.</summary>
  public class CashTransactionEntryDto {

    public long Id {
      get; set;
    }

    public string AccountNumber {
      get; set;
    } = string.Empty;


    public string AccountName {
      get; set;
    } = string.Empty;


    public string ParentAccountFullName {
      get; set;
    } = string.Empty;


    public string SectorCode {
      get; set;
    } = string.Empty;


    public string SubledgerAccountNumber {
      get; set;
    } = string.Empty;


    public string SubledgerAccountName {
      get; set;
    } = string.Empty;


    public string VerificationNumber {
      get; set;
    } = string.Empty;


    public string ResponsibilityAreaCode {
      get; set;
    } = string.Empty;


    public string ResponsibilityAreaName {
      get; set;
    } = string.Empty;


    public string BudgetCode {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public DateTime Date {
      get; set;
    } = ExecutionServer.DateMinValue;


    public int CurrencyId {
      get; set;
    }

    public string CurrencyName {
      get; set;
    } = string.Empty;


    public decimal ExchangeRate {
      get; set;
    }

    public decimal Debit {
      get; set;
    }

    public decimal Credit {
      get; set;
    }

    public int CashAccountId {
      get; set;
    }

    public NamedEntityDto CashAccount {
      get; set;
    }

  }  // class CashTransactionEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
