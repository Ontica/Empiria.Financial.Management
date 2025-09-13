/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.Core.dll        Pattern   : Output DTO                           *
*  Type     : BaseCashEntryDto                              License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger transaction entries.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO used to retrieve cash ledger transaction entries.</summary>
  public class BaseCashEntryDto {

    public long Id {
      get; set;
    }

    public string AccountNumber {
      get; set;
    }

    public string AccountName {
      get; set;
    }

    public string ParentAccountFullName {
      get; set;
    }

    public string SectorCode {
      get; set;
    }

    public string SubledgerAccountNumber {
      get; set;
    }

    public string SubledgerAccountName {
      get; set;
    }

    public string VerificationNumber {
      get; set;
    }

    public string ResponsibilityAreaCode {
      get; set;
    }

    public string ResponsibilityAreaName {
      get; set;
    }

    public string BudgetCode {
      get; set;
    }

    public string Description {
      get; set;
    }

    public DateTime Date {
      get; set;
    }

    public int CurrencyId {
      get; set;
    }

    public string CurrencyName {
      get; set;
    }

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

    public string CashAccountNo {
      get; set;
    }

    public string CuentaSistemaLegado {
      get; set;
    }

    public string CashAccountAppliedRule {
      get; set;
    }

    public int CashAccountRecordedById {
      get; set;
    }

    public DateTime CashAccountRecordingTime {
      get; set;
    }


    public bool CashFlowAssigned {
      get {
        return CashAccountId > 0;
      }
    }


    public bool CashFlowUnassigned {
      get {
        return CashAccountId == CashAccountStatus.CashFlowUnassigned.ControlValue();
      }
    }

    public bool NoCashFlow {
      get {
        return CashAccountId == CashAccountStatus.NoCashFlow.ControlValue();
      }
    }


    public bool Pending {
      get {
        return CashAccountId == CashAccountStatus.Pending.ControlValue();
      }
    }

    #region Temporal Legacy System related properties

    public bool Correct {
      get {
        if (Exact) {
          return false;
        }
        if (Pending) {
          return true;
        }
        if (CashFlowUnassigned && LegadoConFlujo) {
          return true;
        }
        return false;
      }
    }


    public bool Exact {
      get {
        return CashAccountNo == CuentaSistemaLegado;
      }
    }


    public bool FalsePositive {
      get {
        return !Exact && !Correct;
      }
    }


    public bool LegadoConFlujo {
      get {
        return !LegadoSinFlujo;
      }
    }


    public bool LegadoSinFlujo {
      get {
        return CuentaSistemaLegado == CashAccountStatus.NoCashFlow.Name();
      }
    }

    #endregion Temporal Legacy System related properties

  }  // class BaseCashEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
